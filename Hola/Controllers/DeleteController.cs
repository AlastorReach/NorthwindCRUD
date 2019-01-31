using Hola.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hola.Controllers
{
    public class DeleteController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Delete
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Orders(int id)
        {
            Orders order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Json("ok");
        }

        public ActionResult Employees(int id)
        {
            Employees employee = db.Employees.Find(id);
            if(employee == null)
            {
                return HttpNotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Json("ok");
        }

        public ActionResult Customers(int id)
        {
            Customers customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Json("ok");
        }

        public ActionResult Categories(int id)
        {
            Categories category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Json("ok");
        }

        public ActionResult Products(int id)
        {
            Products product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Json("ok");
        }

    }
}