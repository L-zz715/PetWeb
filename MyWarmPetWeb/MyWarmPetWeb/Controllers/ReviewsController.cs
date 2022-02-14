using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWarmPetWeb.Models;

namespace MyWarmPetWeb.Controllers
{
    public class ReviewsController : Controller
    {
        private Entities db = new Entities();
        private string userId;

        // GET: Reviews
        public ActionResult Index()
        {
            var reviewSet = db.ReviewSet.Include(r => r.AspNetUsers).Include(r => r.Vet);
            userId = User.Identity.GetUserId();
            return View(reviewSet.ToList());
        }

        // GET: Reviews/Details/5
        [Authorize(Roles ="Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.ReviewSet.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        [Authorize]
        public ActionResult Create()
        {
            userId = User.Identity.GetUserId();
            ViewBag.AspNetUsersId = userId;
            //ViewBag.AspNetUsersId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReviewTitle,ReviewContent,RateOfVet,VetId")] Review review)
        {
            if (ModelState.IsValid)
            {
                Vet reviewedVet = db.VetSet.Where(v => v.Id == review.VetId).ToList()[0];
                var allReviews = db.ReviewSet.Where(r => r.VetId == review.VetId).ToList();
                double allRate = 0;

                // get all rate at db
                foreach(var item in allReviews)
                {
                    allRate += item.RateOfVet;
                }

                // plus new added rate
                allRate += review.RateOfVet;

                // count the number of review(rate by customer)
                int lonNum = allReviews.Count;
                double currentRate = Math.Round(allRate/ (allReviews.Count + 1),2);

                reviewedVet.Rate = currentRate;
                db.Entry(reviewedVet).State = EntityState.Modified;

                userId = User.Identity.GetUserId();
                review.AspNetUsersId = userId;
                review.AspNetUsers = db.AspNetUsers.Find(userId);
                db.ReviewSet.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.AspNetUsersId = new SelectList(db.AspNetUsers, "Id", "Email", review.AspNetUsersId);
            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName", review.VetId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.ReviewSet.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetUsersId = new SelectList(db.AspNetUsers, "Id", "Email", review.AspNetUsersId);
            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName", review.VetId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReviewTitle,ReviewContent,RateOfVet,AspNetUsersId,VetId")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUsersId = new SelectList(db.AspNetUsers, "Id", "Email", review.AspNetUsersId);
            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName", review.VetId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.ReviewSet.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.ReviewSet.Find(id);
            db.ReviewSet.Remove(review);
            db.SaveChanges();
            Vet reviewedVet = db.VetSet.Where(v => v.Id == review.VetId).ToList()[0];
            var allReviews = db.ReviewSet.Where(r => r.VetId == review.VetId).ToList();
            double allRate = 0;

            // get all rate at db
            foreach (var item in allReviews)
            {
                allRate += item.RateOfVet;
            }
            

            // count the number of review(rate by customer)
            int lonNum = allReviews.Count;
            double currentRate = allRate / allReviews.Count;

            reviewedVet.Rate = currentRate;
            db.Entry(reviewedVet).State = EntityState.Modified;

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
