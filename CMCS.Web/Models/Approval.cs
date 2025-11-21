using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Web.Models
{
    public class Approval
    {
        [Key]
        public int ApprovalId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required]
        public string ActionedByUserId { get; set; } = string.Empty;

        public string RoleAtApproval { get; set; } = string.Empty;

        public string Decision { get; set; } = "Pending";

        public string Comment { get; set; } = string.Empty;

        // 1 = Coordinator, 2 = Manager
        public int Level { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
