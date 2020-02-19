using System;

namespace ArabicWorkersMangement.ViewModels
{
    public class DayViewModel
    {
        public int DayId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DayDate { get; set; }
        public DateTime ShiftStartingTime { get; set; }
        public DateTime ShiftEndingTime { get; set; }
    }
}