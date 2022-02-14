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
using MyWarmPetWeb.Utils;

namespace MyWarmPetWeb.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private Entities db = new Entities();
        public string userId;
        public int VetIDTemp;
        // GET: Bookings

        public ActionResult Index()
        {
            //var bookingSet = db.BookingSet.Include(b => b.AspNetUsers);
            if (User.IsInRole("Admin"))
            {
         
                return View(db.BookingSet.ToList());
            }
            else if (User.IsInRole("PublicUser"))
            {
                userId = User.Identity.GetUserId();
                var bookingSet = db.BookingSet.Include(b => b.AspNetUsers).Where(b => b.AspNetUsersId == userId);
                return View(bookingSet.ToList());
            }

            return HttpNotFound();
          
        }

  

        public ActionResult EmailSent()
        {
            return View();
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.BookingSet.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create   
        public ActionResult Create()
        {
            userId = User.Identity.GetUserId();
            ViewBag.AspNetUsersId = userId;
            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName");
            //ViewBag.AspNetUsersId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // GET: Bookings/CreatePageTwo
        public ActionResult CreatePageTwo(int selectedVetId)
        {
            userId = User.Identity.GetUserId();
            ViewBag.AspNetUserId = userId;

            var ConsultationTimes = db.ConsultationTimeSet.Where(c => c.VetId == selectedVetId && c.BookingStatus.Equals("Free")).ToList();
            var DateData = ConsultationTimes.Select(c => c.DateAndTime);
     
            ViewBag.DateAndTime = new SelectList(DateData);
            Vet selectedVet = db.VetSet.Find(selectedVetId);
            // create a TempData for send variable to other function
            TempData["VetId"] = selectedVetId;

            return View();
        }


        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VetId")] int VetId)
        {
            if (ModelState.IsValid)
            {
                // return a variable selectedVetId to next page CreatePageTwo
                return RedirectToAction("CreatePageTwo", new { selectedVetId = VetId });
            }

            //ViewBag.AspNetUsersId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AspNetUsersId);
            ViewBag.VetId = new SelectList(db.VetSet, "Id", "VetName", VetId);
            return View();
        }


        // POST: Bookings/CreatePageTwo
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePageTwo([Bind(Include = "Id,CustomerName,PetName,PetType,CustomerPhoneNumber,DateAndTime")] Booking booking)
        {
            

            if (ModelState.IsValid)
            {
                // get value from TempData
                int tempVetId = (int)TempData["VetId"];
                // get consultationTime if it matches vetId and dateAndTime
                var ConsultationTimes = db.ConsultationTimeSet.Where(c => c.VetId == tempVetId && c.DateAndTime == booking.DateAndTime).ToList();
                ConsultationTime consultationT = ConsultationTimes[0];

                // change its status, since it is chosen by customer
                consultationT.BookingStatus = "Booked";
                
                userId = User.Identity.GetUserId();
                ViewBag.VetNameO = db.VetSet.Find(tempVetId).VetName;
                //db.SaveChanges();

                // asign value for booking
                booking.AspNetUsersId = userId;
                booking.ConsultationTime = consultationT;
                booking.VetId = (int)TempData["VetId"];
                booking.AspNetUsers = db.AspNetUsers.Find(userId);
                consultationT.Booking = booking;
                TempData["bookOne"] = booking;

                // modify consultationtime table
                db.Entry(consultationT).State = EntityState.Modified;

                //BookId = booking.Id;
                //VetIDPu = booking.VetId;
                //db.Entry(booking).State = EntityState.Modified;

                //Email
                try
                {
                    // save changes
                    db.BookingSet.Add(booking);
                    db.SaveChanges();
               
                    var thisUser = db.AspNetUsers.Where(a => a.Id == userId).ToList();
                    String toEmail = thisUser[0].Email;
                    EmailSender es = new EmailSender();
                    es.Send(toEmail);

                    return RedirectToAction("EmailSent");
                }
                catch
                {
                    
                    return View();
                }

             
            }

            ViewBag.AspNetUserId = userId;
            //ViewBag.VetId = new SelectList(db.Vets, "Id", "VetName", booking.VetId);
            //ViewBag.DateAndTimeT = new SelectList(DateData, booking.DateAndTime);
            return View(booking);
        }





        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.BookingSet.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetUsersId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AspNetUsersId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerName,PetName,PetType,CustomerPhoneNumber")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                

                //ConsultationTime consultationOne = booking.ConsultationTime;
                //ConsultationTime consultationT = ConsultationTimes[0];
                //consultationOne.BookingStatus = "Free";
                //db.Entry(consultationOne).State = EntityState.Modified;


                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUsersId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AspNetUsersId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.BookingSet.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.BookingSet.Find(id);
           

            //var ConsultationTimes = db.ConsultationTimeSet.Where(c => c.VetId == booking.VetId && c.DateAndTime == booking.DateAndTime).ToList();
            ConsultationTime consultationT = booking.ConsultationTime;
            //ConsultationTime consultationT = ConsultationTimes[0];

            // modify consultation time to Free, make it can be booked by other customer
            consultationT.BookingStatus = "Free";
            db.Entry(consultationT).State = EntityState.Modified;
            db.BookingSet.Remove(booking);
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
