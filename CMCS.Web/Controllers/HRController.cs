using CMCS.Web.Data;
using CMCS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Web.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly AppDbContext _db;

        public HRController(AppDbContext db) => _db = db;

        // Summary of claims that reached manager approval
        public async Task<IActionResult> ApprovedClaims()
        {
            var claims = await _db.Claims.Where(c => c.Status == ClaimStatus.ApprovedByManager)
                                         .OrderByDescending(c => c.DateSubmitted)
                                         .ToListAsync();
            return View(claims);
        }

        // Export CSV for payroll
        public async Task<FileResult> ExportApprovedClaimsCsv()
        {
            var claims = await _db.Claims.Where(c => c.Status == ClaimStatus.ApprovedByManager).ToListAsync();

            var csv = new System.Text.StringBuilder();
            csv.AppendLine("ClaimId,LecturerName,HoursWorked,HourlyRate,TotalPayment,DateSubmitted");

            foreach (var c in claims)
            {
                csv.AppendLine($"{c.Id},\"{c.LecturerName}\",{c.HoursWorked},{c.HourlyRate},{c.TotalPayment},{c.DateSubmitted:yyyy-MM-dd}");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "ApprovedClaims.csv");
        }

        // Simple lecturer user list & edit (basic user management demonstration)
        public async Task<IActionResult> Lecturers()
        {
            // Use the Users property directly from IdentityDbContext<User>
            var users = await _db.Users.ToListAsync();
            return View(users);
        }
    }
}

