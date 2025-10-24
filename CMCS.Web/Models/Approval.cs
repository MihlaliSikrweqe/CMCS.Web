using System.ComponentModel.DataAnnotations;

namespace CMCS.Web.Models
{
    public class Approval
    {
        [Key] public int ApprovalId { get; set; }
        public int ClaimId { get; set; }
        public int ApprovedByUserId { get; set; }
        public string RoleAtApproval { get; set; } = string.Empty;
        public string Decision { get; set; } = "Pending";
        public string Comment { get; set; } = string.Empty;
        public int Level { get; set; } // 1=Coordinator, 2=Manager
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
