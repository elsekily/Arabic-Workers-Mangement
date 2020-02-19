
using ArabicWorkersMangement.Models;
using ArabicWorkersMangement.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArabicWorkersMangement.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext context;

        public EmployeeController()
        {
            context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
        [Route("employees")]
        public ActionResult Index()
        {
            var employees = context.Employees.OrderBy(x => x.Name).ToList();
            return View(employees);
        }
        [Route("employee/new")]
        public ActionResult New()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Save(Employee employee)
        {
            if (employee.Name == null)
                return RedirectToAction("new", "Employee");

            if (employee.ID == 0)
            {
                context.Employees.Add(employee);
            }
            else
            {
                var employeeInDB = context.Employees.Single(x => x.ID == employee.ID);

                employeeInDB.Name = employee.Name;
                employeeInDB.SalaryPerHour = employee.SalaryPerHour;
                employeeInDB.PhoneNumber = employee.PhoneNumber;
                employeeInDB.Address = employee.Address;
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Employee");
        }
        [Route("employee/details/{id}")]
        public ActionResult Details(int id)
        {
            var employeeInDb = context.Employees.SingleOrDefault(c => c.ID == id);
            if (employeeInDb == null)
                return HttpNotFound();
            var EmployeeAndDays = new EmployeeDaysViewModel()
            {
                Employee = employeeInDb,

                Days = context.Days.Where(c => c.Employee.ID == id)
                .OrderByDescending(x => x.Start).Take(100).ToList(),

                WeekHours = context.Days.ToList()
               .Where(x => x.Employee.ID == id)
               .GroupBy(g => (CultureInfo.CurrentCulture.Calendar
                .GetWeekOfYear(g.Start.Date, CalendarWeekRule.FirstDay, DayOfWeek.Saturday)))
                .Select(z => new
                {
                    WeekNumber = z.Key,
                    WeekFirstDayDublicate = z.Min
                    (c => c.Start.AddDays(-1 * (7 + (c.Start.DayOfWeek - DayOfWeek.Saturday)) % 7)),
                    TotalHours = z.Sum(c => c.DayDuration),
                })
                .GroupBy(z => z.WeekFirstDayDublicate)
                .Select(z => new WeekHoursViewModel
                {
                    WeekFirstDay = z.Key,
                    TotalHours = z.Sum(c => c.TotalHours)
                })
                .OrderByDescending(z => z.WeekFirstDay).Take(5)
            };
            return View(EmployeeAndDays);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var employee = context.Employees.SingleOrDefault(c => c.ID == id);
            var days = context.Days.Where(x => x.Employee.ID == id).ToList();

            foreach (var day in days)
            {
                context.Days.Remove(day);
            }
            context.Employees.Remove(employee);
            context.SaveChanges();
            return RedirectToAction("Index", "Employee");
        }
    }
}
