using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArabicWorkersMangement.Models
{
    public class Day
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double DayDuration { get; set; }
        public virtual Employee Employee { get; set; }
        public Day()
        {
            Employee = new Employee();
        }
    }
}