using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class TaskItem
    {
        public int TaskId { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } = "Pending";

        // For UI
        public string? EmployeeName { get; set; }
    }
}
