using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class AttendanceRecord
    {
        public int AttendanceId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required, DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime AttendanceDate { get; set; }

        [Required, StringLength(10)]
        public string Status { get; set; }  // Present / Absent / Leave

        [StringLength(250)]
        public string Remarks { get; set; }

        public string? EmployeeName { get; set; }
    }
}
