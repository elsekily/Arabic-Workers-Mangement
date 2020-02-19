using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ArabicWorkersMangement.Models
{
    public class Employee
    {
        public int ID { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public double SalaryPerHour { get; set; }
        [StringLength(25)]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public IList<Day> WorkedDays { get; set; }
        public Employee()
        {
            WorkedDays = new List<Day>();
        }
    }
}