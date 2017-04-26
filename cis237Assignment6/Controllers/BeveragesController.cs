using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237Assignment6.Models;

namespace cis237Assignment6.Controllers
{
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageDGipeEntities db = new BeverageDGipeEntities();

        // GET: Beverages
        public ActionResult Index(string sortOrder)
        {

            //Sort variable 
            var sortBeverages = from s in db.Beverages
                                select s;

            //Check if a header was clicked
            switch (sortOrder)
            {
                case "ID":
                    sortBeverages = sortBeverages.OrderBy(s => s.id);
                    return View(sortBeverages.ToList());
                case "name":
                    sortBeverages = sortBeverages.OrderBy(s => s.name);
                    return View(sortBeverages.ToList());
                case "pack":
                    sortBeverages = sortBeverages.OrderBy(s => s.pack);
                    return View(sortBeverages.ToList());
                case "price":
                    sortBeverages = sortBeverages.OrderBy(s => s.price);
                    return View(sortBeverages.ToList());
                //If no header clicked, continue with filter checks
                default:

   //*******************FILTER**********************************************
                   
                    //Setup a variable to hold the data
                    IEnumerable<Beverage> BeveragesToFilter = db.Beverages;

                    //filter variables
                    string filterId = "";
                    string filterName = "";
                    string filterPack = "";
                    string filterMin = "";
                    string filterMax = "";

                    //Define a min and max for the price
                    int min = 0;
                    int max = 1000;

                    //Check to see if there is a value in the session, and if there is, assign it
                    //to the variable that we setup to hold the value.

                    if (Session["id"] != null && !String.IsNullOrWhiteSpace((string)Session["id"]))
                    {
                        filterId = (string)Session["id"];
                    }

                    if (Session["name"] != null && !String.IsNullOrWhiteSpace((string)Session["name"]))
                    {
                        filterName = (string)Session["name"];
                    }

                    if (Session["pack"] != null && !String.IsNullOrWhiteSpace((string)Session["pack"]))
                    {
                        filterPack = (string)Session["pack"];
                    }

                    if (Session["min"] != null && !String.IsNullOrWhiteSpace((string)Session["min"]))
                    {
                        filterMin = (string)Session["min"];
                        min = Int32.Parse(filterMin);
                    }

                    if (Session["max"] != null && !String.IsNullOrWhiteSpace((string)Session["max"]))
                    {
                        filterMax = (string)Session["max"];
                        max = Int32.Parse(filterMax);
                    }

                    //Do the filtering based on values
                    IEnumerable<Beverage> filtered = BeveragesToFilter.Where(beverage => beverage.price >= min &&
                                                                          beverage.price <= max &&
                                                                          beverage.name.Contains(filterName) &&
                                                                          beverage.pack.Contains(filterPack) &&
                                                                          beverage.id.StartsWith(filterId));

                    //Convert the dataset to a list 
                    IEnumerable<Beverage> finalFiltered = filtered.ToList();

                    //Returns inputted values to the text boxes
                    ViewBag.filterId = filterId;
                    ViewBag.filterName = filterName;
                    ViewBag.filterPack = filterPack;
                    ViewBag.filterMin = filterMin;
                    ViewBag.filterMax = filterMax;

                    //Return the view with the filtered selection of cars.
                    return View(finalFiltered);
            }
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                bool search = Search(beverage);

                if (search == true)
                {
                    db.Beverages.Add(beverage);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else

                    ViewBag.Error = "An item with that ID already exists!";
                    return View(beverage);
            }

            return View(beverage);
        }

        private bool Search(Beverage search)
        {

            if (db.Beverages.Find(search.id) == null)
                return true;
            else
                return false;
        }


        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            

            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            //Get the form data that we sent out of the request object.
            string id = Request.Form.Get("id");
            string name = Request.Form.Get("name");
            string pack = Request.Form.Get("pack");
            string min = Request.Form.Get("min");
            string max = Request.Form.Get("max");

            //Assign data from request object into the session so that other methods can have access to it.
            Session["id"] = id;
            Session["name"] = name;
            Session["pack"] = pack;
            Session["min"] = min;
            Session["max"] = max;


            //Redirect to the index page
            return RedirectToAction("Index");
        }
        
    }
}
