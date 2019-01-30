using Hola.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hola.Controllers
{
    public class EditController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Edit
        [HttpGet]
        public ActionResult Employees(int id)
        {
            var employee = (from e in db.Employees where e.EmployeeID == id select e).FirstOrDefault();
            if (employee != null)
            {
                return View(employee);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Employees(Employees e)
        {
            db.Entry(e).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return View(e);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult Orders(int id)
        {
            var orderFound = (from order in db.Orders where order.OrderID == id select order).FirstOrDefault();
            if(orderFound != null)
            {
                return View(orderFound);
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }

        [HttpPost]
        public ActionResult Orders(Orders o)
        {
            db.Entry(o).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return View(o);
            }
            catch (Exception ex)
            {
               if(ex is SqlException || ex is DbUpdateException)
                {
                    return RedirectToAction("Orders", "Edit", "There was an error");
                }
                throw;
            }
        }

        [HttpGet]
        public ActionResult Categories(int id)
        {
            var categoryFound = (from category in db.Categories where category.CategoryID == id select category).FirstOrDefault();
            if(categoryFound != null)
            {
                return View(categoryFound);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Categories(Categories c)
        {
            db.Entry(c).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return View(c);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        Action<Exception> errorHandler = (ex) => {
            Console.WriteLine("Ocurrio una exception");
        };
    }
}