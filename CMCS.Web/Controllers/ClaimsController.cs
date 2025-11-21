using CMCS.Web.Data;
using CMCS.Web.Hubs;
using CMCS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Web.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class ClaimsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ClaimsHub> _hub;

        public ClaimsController(AppDbContext db, UserManager<User> userManager, IHubContext<ClaimsHub> hub)
        {
            _db = db;
            _userManager = userManager;
            _hub = hub;
        }

        // Lecturer: view own claims
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var claims = await _db.Claims.Where(c => c.UserId == userId)
                                         .OrderByDescending(c => c.DateSubmitted)
                                         .ToListAsync();
            return View(claims);
        }

        // GET: Create
        public IActionResult Create() => View();

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim model, IFormFile document)
        {
            // server-side ensure HoursWorked & HourlyRate > 0
            if (model.HoursWorked <= 0) ModelState.AddModelError(nameof(model.HoursWorked), "Hours must be greater than 0.");
            if (model.HourlyRate <= 0) ModelState.AddModelError(nameof(model.HourlyRate), "Hourly rate must be greater than 0.");

            if (!ModelState.IsValid) return View(model);

            model.UserId = _userManager.GetUserId(User);

            // server-side auto-calc (authoritative)
            model.TotalPayment = Decimal.Round((decimal)model.HoursWorked * model.HourlyRate, 2, MidpointRounding.AwayFromZero);

            if (document != null && document.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(document.FileName)}";
                var filePath = Path.Combine(uploads, uniqueFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await document.CopyToAsync(stream);

                model.DocumentPath = $"/uploads/{uniqueFileName}";
            }

            _db.Claims.Add(model);
            await _db.SaveChangesAsync();

            // broadcast real-time notification
            await _hub.Clients.All.SendAsync("ReceiveClaimUpdate", $"New claim submitted by {model.LecturerName}");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            // ensure lecturer only views own claim
            var userId = _userManager.GetUserId(User);
            if (claim.UserId != userId) return Forbid();

            return View(claim);
        }
    }
}


