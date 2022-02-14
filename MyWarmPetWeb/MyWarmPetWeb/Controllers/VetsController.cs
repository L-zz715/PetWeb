using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using MyWarmPetWeb.Models;
using Newtonsoft.Json;


namespace MyWarmPetWeb.Controllers
{
    public class VetsController : Controller
    {
        private Entities db = new Entities();

        // GET: Vets
        public ActionResult Index()
        {
            var vetList1 = db.VetSet.ToList();
            var bookingList = db.BookingSet.ToList();
            var data1 = new Dictionary<int, int>();
            List<int> tempCount = new List<int>();
            List<String> tempVetNames = new List<string>();


            // based on VetId, create a dict with key: VetId, value: the number of times
            // it shows the number of times each vet's consultation is booked
            foreach (var item in bookingList)
            {
                if (!data1.ContainsKey(item.VetId))
                {
                    data1.Add(item.VetId, 1);
                }
                else
                {
                    data1[item.VetId] = data1[item.VetId] + 1;
                }

            }

            // based on Vet name, create a dict with key: VetName, value: the number of times
            // it shows the number of times each vet's consultation is booked
            var dictNew = new Dictionary<String, int>();
            foreach (var item in vetList1)
            {
                if (!dictNew.ContainsKey(item.VetName))
                {
                    if (data1.ContainsKey(item.Id))
                    {
                        dictNew.Add(item.VetName, data1[item.Id]);
                    }
                    else
                    {
                        dictNew.Add(item.VetName, 0);
                    }
                }
                else
                {
                    break;

                }
            }

            // create a list of strimg, for create Json string
            String label1 = "VetName";
            String label2 = "Count";
            List<String> newJs = new List<String>();
            foreach (var item in dictNew)
            {
                String temps = "{'" + label1 + "':'" + item.Key + "','" + label2 + "':" + item.Value + "}";
                newJs.Add(temps);
            }

            String strForJson ="[" +  string.Join(",", newJs) + "]";
           
            // Json string 
            ViewBag.jsonStr = JsonConvert.SerializeObject(strForJson);
   
            return View(db.VetSet.ToList());
        }

        // GET: Vets/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vet vet = db.VetSet.Find(id);
            if (vet == null)
            {
                return HttpNotFound();
            }
            return View(vet);
        }

        // GET: Vets/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VetName,EmailAddress,PhoneNumber,Rate")] Vet vet)
        {
            if (ModelState.IsValid)
            {
                db.VetSet.Add(vet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vet);
        }

        // GET: Vets/Edit/5
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vet vet = db.VetSet.Find(id);
            if (vet == null)
            {
                return HttpNotFound();
            }
            return View(vet);
        }

        // POST: Vets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VetName,EmailAddress,PhoneNumber,Rate")] Vet vet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vet);
        }

        // GET: Vets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vet vet = db.VetSet.Find(id);
            if (vet == null)
            {
                return HttpNotFound();
            }
            return View(vet);
        }

        // POST: Vets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vet vet = db.VetSet.Find(id);
            db.VetSet.Remove(vet);
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
    }
}
