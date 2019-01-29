using Ganss.XSS;
using Hola.Models.DataModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
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
        [HttpGet]
        public ActionResult ListByEntity()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchEntity(string entityName, int currentPage)
        {
            var sanitizer = new HtmlSanitizer();
            string entitySanitinized = sanitizer.Sanitize(entityName);
            var list = this.findEntityAndReturnData(entitySanitinized, currentPage, 10);
            if(list != null)
            {
                return Json(list);
            }
            else
            {
                return Json("Nothing found");
            }
            
        }

        private Object findEntityAndReturnData(string entity, int currentPage, int pageSize)
        {
            List<string> lista = new List<string>() { "Employees", "Categories", "Customers", "Orders" };
            MatchCollection matches = null;
            string match = "";
            IQueryable<Object> entityReturned = Enumerable.Empty<Object>().AsQueryable();
            for(int i = 0; i < lista.Count; i++)
            {
                matches =  Regex.Matches(lista[i], entity, RegexOptions.IgnoreCase);
                if(matches.Count > 0)
                {
                    match = lista[i];
                    break;
                }
            }
            switch (match)
            {
                case "Employees": return (from employee in db.Employees select new
                {
                    Id = employee.EmployeeID,
                    LastName = employee.LastName,
                    FirstName = employee.FirstName,
                    Title = employee.Title,
                    Courtesy = employee.TitleOfCourtesy,
                    BirthDate = employee.BirthDate,
                    HireDate = employee.HireDate,
                    Address= employee.Address,
                    City = employee.City,
                    PostalCode = employee.PostalCode,
                    Country = employee.Country,
                    Entity = "Employees"
                }).ToList();
                case "Categories":
                    return (from category in db.Categories
                            select new
                            {
                                Id = category.CategoryID,
                                Name = category.CategoryName,
                                Description = category.Description,
                                Entity = "Categories"
                            }).ToList();

                case "Orders":
                    return (from order in db.Orders.OrderBy(a => a.OrderID)
                            select new
                            {
                                Id = order.OrderID,
                                Customer = order.CustomerID,
                                Employee = order.EmployeeID,
                                OrderDate = order.OrderDate,
                                RequiredDate = order.RequiredDate,
                                ShippedDate = order.ShippedDate,
                                ShipVia = order.ShipVia,
                                Freight = order.Freight,
                                ShipName = order.ShipName,
                                ShipAddress = order.ShipAddress,
                                ShipCity = order.ShipCity,
                                ShipRegion = order.ShipRegion,
                                ShipCountry = order.ShipCountry,
                                Entity = "Orders"
                            }).Skip((currentPage - 1) * 10).Take(10).ToList();

                default: return null;
            }
        }

    }
}