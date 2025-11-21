using System.ComponentModel.DataAnnotations;

namespace CMCS.Web.Models
{
    public class Claim
    {
        public int Id { get; set; }

        // Who submitted (FK to Identity user)
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required, Display(Name = "Lecturer Name")]
        public string LecturerName { get; set; } = string.Empty;

        [Required, Display(Name = "Hours Worked"), Range(0.01, 1000, ErrorMessage = "Enter a valid number of hours (>0).")]
        public double HoursWorked { get; set; }

        [Required, Display(Name = "Hourly Rate"), Range(0.01, 1000000, ErrorMessage = "Enter a valid rate (>0).")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Total Payment")]
        public decimal TotalPayment { get; set; } // server-calculated

        [Display(Name = "Notes"), StringLength(2000)]
        public string? Notes { get; set; }

        [Display(Name = "Document Path")]
        public string? DocumentPath { get; set; }

        [Required]
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
    }

    public enum ClaimStatus
    {
        Pending = 0,
        ApprovedByCoordinator = 1,
        ApprovedByManager = 2,
        Rejected = 3,
        Paid = 4
    }
}


