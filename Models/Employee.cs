using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Phone, StringLength(20)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [Required, DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }
    }
}
