using ArabicWorkersMangement.Models;
using ArabicWorkersMangement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ArabicWorkersMangement.Controllers
{
    public class DayController : Controller
    {
        private ApplicationDbContext context;

        public DayController()
        {
            context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
        [Route("day/create/{EmployeeID}")]
        public ActionResult Create(int employeeId)
        {
            var today = DateTime.Today;
            var newDay = new DayViewModel()
            {
                DayId = 0,
                EmployeeId = employeeId,
                DayDate = today,
                ShiftStartingTime = new DateTime(today.Year, today.Month, today.Day, 9, 0, 0),
                ShiftEndingTime = new DateTime(today.Year, today.Month, today.Day, 19, 0, 0)
            };
            return View("DayForm", newDay);
        }

        [Route("day/edit/{dayId}")]
        public ActionResult Edit(int dayId)
        {
            try
            {
                var dayInDb = context.Days.Include(c => c.Employee).Where(x => x.Id == dayId).First();
                var editDay = new DayViewModel()
                {
                    DayId = dayInDb.Id,
                    EmployeeId = dayInDb.Employee.ID,
                    DayDate = dayInDb.Start.Date,
                    ShiftStartingTime = dayInDb.Start,
                    ShiftEndingTime = dayInDb.End
                };
                return View("DayForm", editDay);
            }
            catch
            {
                return RedirectToAction("Index", "Employee");
            }
        }
        [HttpPost]
        public ActionResult Save(DayViewModel day)
        {
            if (day.ShiftStartingTime >= day.ShiftEndingTime)
                return RedirectToAction("details", "Employee", new { id = day.EmployeeId });

            if (day.DayId == 0)
            {
                var newDay = new Day()
                {
                    Employee = context.Employees.First(x => x.ID == day.EmployeeId),
                    Start = SettingTime(day.DayDate, day.ShiftStartingTime),
                    End = SettingTime(day.DayDate, day.ShiftEndingTime)
                };
                newDay.DayDuration = (newDay.End - newDay.Start).TotalHours;
                context.Days.Add(newDay);
            }
            else
            {
                var dayInDb = context.Days.Include(c => c.Employee).Where(x => x.Id == day.DayId).First();
                dayInDb.Start = SettingTime(day.DayDate, day.ShiftStartingTime);
                dayInDb.End = SettingTime(day.DayDate, day.ShiftEndingTime);
                dayInDb.DayDuration = (dayInDb.End - dayInDb.Start).TotalHours;
            }
            context.SaveChanges();
            return RedirectToAction("details", "Employee", new { id = day.EmployeeId });
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var day = context.Days.Include(c => c.Employee).SingleOrDefault(c => c.Id == id);
            var employeeId = day.Employee.ID;
            context.Days.Remove(day);
            context.SaveChanges();
            return RedirectToAction("details", "Employee", new { id = employeeId });
        }
        private DateTime SettingTime(DateTime date, DateTime time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 00);
        }
    }
}