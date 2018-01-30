using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MeetingRoomBooking.Models;

namespace MeetingRoomBooking.Controllers
{
    public class BookingsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        static BookingsController()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");            
        }
        void ClearDB()
        {
            db.Bookings.RemoveRange(db.Bookings);
            db.SaveChanges();
        }
        void AddRooms()
        {
            db.Rooms.Add(new Room { RoomName = "Board Room" });
            db.Rooms.Add(new Room { RoomName = "Person Vue" });
            db.Rooms.Add(new Room { RoomName = "Conf. Room" });
            db.Rooms.Add(new Room { RoomName = "SoftLab" });
            db.Rooms.Add(new Room { RoomName = "MakersLab" });
            db.SaveChanges();
        }
        // GET: Bookings
        public ActionResult Index()
        {            
            ViewBag.RoomList = db.Rooms.ToList();         
            return View(db.Bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BookingName,OwnerName,BookingDay,BookingStart,BookingEnd")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BookingName,OwnerName,BookingDay,BookingStart,BookingEnd")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
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
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<string> GetAvailableStartHourFromDay(DateTime day,TimeSpan startHour, int roomID)
        {
            List<string> availableHourList = null;

            List<Booking> BookingRoomDayList = db.Bookings.Where(b => b.BookingDay == day && b.Room.ID == roomID).ToList();
            for (TimeSpan ts = startHour; ts < new TimeSpan(17, 0, 0); ts.Add(new TimeSpan(0, 30, 0)))
            {
                if (BookingRoomDayList.Any(b => isTimeBetween(ts, b.BookingStart, b.BookingEnd)))
                    return availableHourList;
                else
                    availableHourList.Add(ts.ToString("HH:mm"));
            }            

            return availableHourList;
        }

        public List<string> GetAvailableEndHourFromDay(DateTime day, int roomID)
        {
            List<string> availableHourList = null;

            List<Booking> BookingRoomDayList = db.Bookings.Where(b => b.BookingDay == day && b.Room.ID == roomID).ToList();
            for (TimeSpan ts = new TimeSpan(09, 0, 0); ts < new TimeSpan(17, 0, 0); ts.Add(new TimeSpan(0, 30, 0)))
            {
                if (!BookingRoomDayList.Any(b => isTimeBetween(ts, b.BookingStart, b.BookingEnd)))
                    availableHourList.Add(ts.ToString("HH:mm"));
            }

            return availableHourList;
        }

        public bool isTimeBetween(TimeSpan time, TimeSpan start, TimeSpan end)
        {
            return start <= time && time <= end;
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
