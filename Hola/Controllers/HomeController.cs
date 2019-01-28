using Hola.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Hola.Controllers
{
    public class HomeController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Employees()
        {
            var employees = (from employee in db.Employees.OrderBy(a => a.EmployeeID) select employee).AsEnumerable();
            return View(employees);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var employee = (from e in db.Employees where e.EmployeeID == id select e).FirstOrDefault();
            if(employee != null)
            {
                return View(employee);
            }
            else
            {
                return HttpNotFound();
            }
            
        }
        [HttpPost]
        public ActionResult Edit(Employees employee)
        {
          
            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return View(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var employee = (from e in db.Employees where e.EmployeeID == id select e).SingleOrDefault();
            if (employee != null)
            {
                return View(employee);
            }
            else
            {
                return HttpNotFound("There was not any employee that matches id");
            }
        }
    }
}