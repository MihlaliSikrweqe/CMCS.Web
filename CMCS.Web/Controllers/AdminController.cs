using Microsoft.AspNetCore.Mvc;
using CMCS.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using CMCS.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using CMCS.Web.Models; // <-- Add this if ClaimStatus is in this namespace
using Microsoft.EntityFrameworkCore; // <-- Add this using directive

namespace CMCS.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ClaimsHub> _hub;

        public AdminController(AppDbContext context, IHubContext<ClaimsHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims.ToListAsync();
            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.ApprovedByManager;
            await _context.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("ReceiveClaimUpdate", "Claim approved.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
            await _context.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("ReceiveClaimUpdate", "Claim rejected.");
            return RedirectToAction(nameof(Index));
        }
    }
}

