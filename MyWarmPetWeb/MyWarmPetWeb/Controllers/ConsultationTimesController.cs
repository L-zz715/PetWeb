using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyWarmPetWeb.Models;

namespace MyWarmPetWeb.Controllers
{
    public class ConsultationTimesController : Controller
    {
        private Entities db = new Entities();

        // GET: ConsultationTimes
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var consultationTimeSet = db.ConsultationTimeSet.Include(c => c.Vet).Include(c => c.Booking);
            return View(consultationTimeSet.ToList());
        }

        // GET: ConsultationTimes/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultationTime consultationTime = db.ConsultationTimeSet.Find(id);
            if (consultationTime == null)
            {
                return HttpNotFound();
            }
            return View(consultationTime);
        }

        // GET: ConsultationTimes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName");
            ViewBag.Id = new SelectList(db.BookingSet, "Id", "CustomerName");
            return View();
        }

        // POST: ConsultationTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateAndTime,BookingStatus,VetId")] ConsultationTime consultationTime)
        {
            if (ModelState.IsValid)
            {
                db.ConsultationTimeSet.Add(consultationTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName", consultationTime.VetId);
            ViewBag.Id = new SelectList(db.BookingSet, "Id", "CustomerName", consultationTime.Id);
            return View(consultationTime);
        }

        // GET: ConsultationTimes/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultationTime consultationTime = db.ConsultationTimeSet.Find(id);
            if (consultationTime == null)
            {
                return HttpNotFound();
            }
            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName", consultationTime.VetId);
            ViewBag.Id = new SelectList(db.BookingSet, "Id", "CustomerName", consultationTime.Id);
            return View(consultationTime);
        }

        // POST: ConsultationTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateAndTime,BookingStatus,VetId")] ConsultationTime consultationTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultationTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName", consultationTime.VetId);
            ViewBag.Id = new SelectList(db.BookingSet, "Id", "CustomerName", consultationTime.Id);
            return View(consultationTime);
        }

        // GET: ConsultationTimes/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultationTime consultationTime = db.ConsultationTimeSet.Find(id);
            if (consultationTime == null)
            {
                return HttpNotFound();
            }
            return View(consultationTime);
        }

        // POST: ConsultationTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConsultationTime consultationTime = db.ConsultationTimeSet.Find(id);
            db.ConsultationTimeSet.Remove(consultationTime);
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
