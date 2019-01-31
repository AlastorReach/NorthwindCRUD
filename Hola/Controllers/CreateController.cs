using Hola.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hola.Controllers
{
    public class CreateController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Employees()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Employees(Employees e)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json("BadRequest");
                }

                db.Employees.Add(e);
                db.SaveChanges();

                return View(e);
            }
            catch(Exception ex)
            {
                if(ex is DbEntityValidationException)
                {
                    return View(e);
                }
                throw;
            }
        }
    }
}