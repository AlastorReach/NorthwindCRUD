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
    }
}