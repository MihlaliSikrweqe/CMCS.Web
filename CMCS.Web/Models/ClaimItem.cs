using System.ComponentModel.DataAnnotations;

namespace CMCS.Web.Models
{
    public class ClaimItem
    {
        [Key] public int ClaimItemId { get; set; }
        public int ClaimId { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal HoursWorked { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
