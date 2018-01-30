using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeetingRoomBooking.Models;

namespace MeetingRoomBooking.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext db = new MyDbContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agenda()
        {

            return View(db.Bookings.ToList().OrderBy(b => b.BookingStart).OrderBy(b => b.RoomId).OrderBy(b=>b.BookingDay));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(db.Rooms.ToList());
        }

        [HttpPost]
        public ActionResult Create(Booking b)
        {
            db.Bookings.Add(b);
            db.SaveChanges();
            return View(db.Rooms.ToList());
        }

        public ActionResult GetAvailableEndHourFromDay(string dayString, string startHourString, int roomID)
        {
            DateTime day = DateTime.Parse(dayString);
            TimeSpan startHour = TimeSpan.Parse(startHourString);
            if (dayString == null || startHour == null)
                return null;

            List<string> availableHourList = new List<string>();

            List<Booking> BookingRoomDayList = db.Bookings.Where(b => b.BookingDay == day && b.Room.RoomID == roomID).ToList();
            for (TimeSpan ts = startHour+= new TimeSpan(0, 30, 0); ts <= new TimeSpan(17, 0, 0); ts+=(new TimeSpan(0, 30, 0)))
            {
                if(BookingRoomDayList.Any(b => b.BookingStart < ts && b.BookingEnd >= ts))
                    return Json(availableHourList);
                else
                    availableHourList.Add(ts.ToString(@"hh\:mm"));
            }

            return Json(availableHourList);
        }

        public ActionResult GetAvailableStartHourFromDay(string dayString, int roomID)
        {
            DateTime day = DateTime.Parse(dayString);
            if (day == null)
                return null;

            List<string> availableHourList = new List<string>();

            List<Booking> BookingRoomDayList = db.Bookings.Where(b => b.BookingDay == day && b.Room.RoomID == roomID).ToList();
            for (TimeSpan ts = new TimeSpan(9, 0, 0); ts < new TimeSpan(17, 0, 0); ts+=new TimeSpan(0, 30, 0))
            {
                if (!BookingRoomDayList.Any(b => b.BookingStart<=ts && b.BookingEnd > ts))
                    availableHourList.Add(ts.ToString(@"hh\:mm"));
            }

            return Json(availableHourList);
        }

        [Obsolete]
        public bool isTimeBetween(TimeSpan time, TimeSpan start, TimeSpan end)
        {
            return start < time && time < end;
        }
    }
}