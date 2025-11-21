using Microsoft.AspNetCore.Identity;

namespace CMCS.Web.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        // Add lecturer-specific fields if needed (BankAccountNumber, StaffNumber)
        public string? StaffNumber { get; set; }
    }
}
