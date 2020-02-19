using ArabicWorkersMangement.Models;
using System.Collections.Generic;

namespace ArabicWorkersMangement.ViewModels
{
    public class EmployeeDaysViewModel
    {
        public Employee Employee { get; set; }
        public IEnumerable<Day> Days { get; set; }
        public IEnumerable<WeekHoursViewModel> WeekHours { get; set; }
    }
}