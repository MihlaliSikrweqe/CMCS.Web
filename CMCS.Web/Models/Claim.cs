using System;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Web.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lecturer Name")]
        public string LecturerName { get; set; }

        [Required]
        [Display(Name = "Hours Worked")]
        public double HoursWorked { get; set; }

        [Required]
        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Document")]
        public string? DocumentPath { get; set; }

        public string Status { get; set; } = "Pending";

        [Display(Name = "Date Submitted")]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;
    }
}


