using Microsoft.AspNetCore.Mvc;
using CMCS.Web.Data;
using CMCS.Web.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using CMCS.Web.Hubs;

namespace CMCS.Web.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ClaimsHub> _hub;

        public ClaimsController(AppDbContext context, IHubContext<ClaimsHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public IActionResult Index()
        {
            var claims = _context.Claims.ToList();
            return View(claims);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim claim, IFormFile document)
        {
            if (ModelState.IsValid)
            {
                if (document != null && document.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/uploads", document.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(stream);
                    }
                    claim.DocumentPath = "/uploads/" + document.FileName;
                }

                _context.Add(claim);
                await _context.SaveChangesAsync();
                await _hub.Clients.All.SendAsync("ReceiveClaimUpdate", "New claim submitted.");

                return RedirectToAction(nameof(Index));
            }
            return View(claim);
        }

        public IActionResult Details(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();
            return View(claim);
        }
    }
}

