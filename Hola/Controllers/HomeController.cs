using Ganss.XSS;
using Hola.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SearchEntity(FormCollection formCollection)
        {
            if (!ModelState.IsValid)
            {
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Not Valid");
            }
            var entityName = formCollection["entityName"];
            int currentPage = int.Parse(formCollection["currentPage"]);
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
            MatchCollection matches = null;
            string match = "";
            IQueryable<Object> entityReturned = Enumerable.Empty<Object>().AsQueryable();
            for(int i = 0; i < Hola.Resources.Resources.entityList.Count(); i++)
            {
                matches =  Regex.Matches(Hola.Resources.Resources.entityList[i], entity, RegexOptions.IgnoreCase);
                if(matches.Count > 0)
                {
                    match = Hola.Resources.Resources.entityList[i];
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

                case "Products":
                    return (from product in db.Products.OrderBy(a => a.ProductID)
                            select new
                            {
                                productId = product.ProductID,
                                productName = product.ProductName,
                                supplier = product.SupplierID,
                                category = product.CategoryID,
                                qPerUnit = product.QuantityPerUnit,
                                unitPrice = product.UnitPrice,
                                inStock = product.UnitsInStock,
                                onOrder = product.UnitsOnOrder,
                                reorderLevel = product.ReorderLevel,
                                discontinued = product.Discontinued,
                                Entity = "Products",
                            }).Skip((currentPage - 1) * 10).Take(10).ToList();

                case "Region": return (from region in db.Region.OrderBy(r => r.RegionID)
                                       select new
                                       {
                                           RegionId = region.RegionID,
                                           Description = region.RegionDescription
                                       }).Skip((currentPage - 1) * 10).Take(10).ToList();
                case "Shippers": return (from s in db.Shippers.OrderBy(s => s.ShipperID)
                                         select new
                                         {
                                             ShipperID = s.ShipperID,
                                             CompanyName = s.CompanyName,
                                             Phone = s.Phone
                                         }).Skip((currentPage - 1) * 10).Take(10).ToList();
                case "Suppliers": return (from s in db.Suppliers.OrderBy( a => a.SupplierID) select
                                          new
                                          {
                                              SupplierID = s.SupplierID,
                                              CompanyName = s.CompanyName,
                                              ContactName = s.ContactName,
                                              ContactTitle = s.ContactTitle,
                                              Address = s.Address,
                                              City = s.City,
                                              Region = s.Region,
                                              PostalCode = s.PostalCode,
                                              Country = s.Country,
                                              Phone = s.Phone
                                          }).Skip((currentPage - 1) * 10).Take(10).ToList();

                case "Territories": return (from t in db.Territories.OrderBy( a => a.TerritoryID) select
                                            new
                                            {
                                                TerritoryID = t.TerritoryID,
                                                Description = t.TerritoryDescription,
                                                RegionID = t.RegionID,
                                            }).Skip((currentPage - 1) * 10).Take(10).ToList();
                default: return null;
            }
        }

    }
}