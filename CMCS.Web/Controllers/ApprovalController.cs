using CMCS.Web.Data;
using CMCS.Web.Hubs;
using CMCS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Web.Controllers
{
    [Authorize(Roles = "Coordinator,Manager")]
    public class ApprovalController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ClaimsHub> _hub;

        public ApprovalController(AppDbContext db, UserManager<User> userManager, IHubContext<ClaimsHub> hub)
        {
            _db = db;
            _userManager = userManager;
            _hub = hub;
        }

        // Coordinator sees Pending claims
        [Authorize(Roles = "Coordinator")]
        public async Task<IActionResult> PendingCoordinator()
        {
            var list = await _db.Claims.Where(c => c.Status == ClaimStatus.Pending).ToListAsync();
            return View(list);
        }

        // Coordinator posts approval
        [HttpPost]
        [Authorize(Roles = "Coordinator")]
        public async Task<IActionResult> ApproveAsCoordinator(int id, string? comment)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.ApprovedByCoordinator;
            _db.Approvals.Add(new Approval
            {
                ClaimId = id,
                ActionedByUserId = _userManager.GetUserId(User),
                RoleAtApproval = "Coordinator",
                Decision = "Approved",
                Comment = comment,
                Level = 1
            });
            await _db.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("ReceiveClaimUpdate", $"Claim #{id} approved by Coordinator");
            return RedirectToAction(nameof(PendingCoordinator));
        }

        [HttpPost]
        [Authorize(Roles = "Coordinator")]
        public async Task<IActionResult> RejectAsCoordinator(int id, string? comment)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
            _db.Approvals.Add(new Approval
            {
                ClaimId = id,
                ActionedByUserId = _userManager.GetUserId(User),
                RoleAtApproval = "Coordinator",
                Decision = "Rejected",
                Comment = comment,
                Level = 1
            });

            await _db.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("ReceiveClaimUpdate", $"Claim #{id} rejected by Coordinator");
            return RedirectToAction(nameof(PendingCoordinator));
        }

        // Manager sees claims approved by Coordinator
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PendingManager()
        {
            var list = await _db.Claims.Where(c => c.Status == ClaimStatus.ApprovedByCoordinator).ToListAsync();
            return View(list);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveAsManager(int id, string? comment)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.ApprovedByManager;
            _db.Approvals.Add(new Approval
            {
                ClaimId = id,
                ActionedByUserId = _userManager.GetUserId(User),
                RoleAtApproval = "Manager",
                Decision = "Approved",
                Comment = comment,
                Level = 2
            });

            await _db.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("ReceiveClaimUpdate", $"Claim #{id} approved by Manager");
            return RedirectToAction(nameof(PendingManager));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RejectAsManager(int id, string? comment)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
            _db.Approvals.Add(new Approval
            {
                ClaimId = id,
                ActionedByUserId = _userManager.GetUserId(User),
                RoleAtApproval = "Manager",
                Decision = "Rejected",
                Comment = comment,
                Level = 2
            });

            await _db.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("ReceiveClaimUpdate", $"Claim #{id} rejected by Manager");
            return RedirectToAction(nameof(PendingManager));
        }
    }
}

