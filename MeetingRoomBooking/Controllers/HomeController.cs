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

        public ActionResult Create()
        {
            return View(db.Rooms.ToList());
        }

        public ActionResult GetAvailableEndHourFromDay(string dayString, string startHourString, int roomID)
        {
            DateTime day = DateTime.Parse(dayString);
            TimeSpan startHour = TimeSpan.Parse(startHourString);
            if (dayString == null||startHour==null)
                return null;

            List<string> availableHourList = null;

            List<Booking> BookingRoomDayList = db.Bookings.Where(b => b.BookingDay == day && b.Room.ID == roomID).ToList();
            for (TimeSpan ts = startHour; ts < new TimeSpan(17, 0, 0); ts.Add(new TimeSpan(0, 30, 0)))
            {
                if (BookingRoomDayList.Any(b => isTimeBetween(ts, b.BookingStart, b.BookingEnd)))
                    return Json(availableHourList);
                else
                    availableHourList.Add(ts.ToString("HH:mm"));
            }

            return Json(availableHourList);
        }

        public ActionResult GetAvailableStartHourFromDay(string dayString, int roomID)
        {
            if (dayString == null)
                return null;

            DateTime day = DateTime.Parse(dayString);

            List<string> availableHourList = new List<string>();

            List<Booking> BookingRoomDayList = db.Bookings.Where(b => b.BookingDay == day && b.Room.ID == roomID).ToList();
            for (TimeSpan ts = new TimeSpan(09, 0, 0); ts < new TimeSpan(17, 0, 0); ts+=new TimeSpan(0, 30, 0))
            {
                if (!BookingRoomDayList.Any(b => isTimeBetween(ts, b.BookingStart, b.BookingEnd)))
                    availableHourList.Add(ts.ToString(@"hh\:mm"));
                Debug.WriteLine("Current timespan = " + ts.ToString());
            }
            Debug.WriteLine("Out of loop");
            return Json(availableHourList,JsonRequestBehavior.AllowGet);
        }

        public bool isTimeBetween(TimeSpan time, TimeSpan start, TimeSpan end)
        {
            return start <= time && time <= end;
        }
    }
}