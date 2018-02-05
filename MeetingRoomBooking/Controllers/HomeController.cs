using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MeetingRoomBooking.Models;
using MeetingRoomBooking.TokenStorage;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace MeetingRoomBooking.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext db = new MyDbContext();
        // GET: Home
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                string userName = ClaimsPrincipal.Current.FindFirst("name").Value;
                string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userId))
                {
                    // Invalid principal, sign out
                    return RedirectToAction("SignOut");
                }

                // Since we cache tokens in the session, if the server restarts
                // but the browser still has a cached cookie, we may be
                // authenticated but not have a valid token cache. Check for this
                // and force signout.
                SessionTokenCache tokenCache = new SessionTokenCache(userId, HttpContext);
                if (!tokenCache.HasData())
                {
                    // Cache is empty, sign out
                    return RedirectToAction("SignOut");
                }

                ViewBag.UserName = userName;

                return new RedirectResult("/Home/Agenda");
            }
            return new RedirectResult("/Home/SignIn");
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

            Models.BaseEventRequestBody requestBody =
                new BaseEventRequestBody(
                    b.OwnerName + ": " + b.BookingName,
                    b.BookingDay.Add(b.BookingStart).ToUniversalTime().ToLongTimeString(),
                    b.BookingDay.Add(b.BookingEnd).ToUniversalTime().AddHours(3).ToLongTimeString(),
                    "body",
                    db.Rooms.ToList().Find(r=> r.RoomID == b.RoomId).RoomName,
                    b.OwnerName
                );

            var status = GraphAPILib.Helpers.AddEvent(HttpContext, requestBody).Result;
            
            return View("Agenda");
        }

        [HttpPost]
        public async Task<ActionResult> AddEvent(string douze)
        {
            Models.BaseEventRequestBody requestBody =
                new BaseEventRequestBody(
                    "MySubject",
                    DateTime.Now.ToUniversalTime().ToLongTimeString(),
                    DateTime.Now.ToUniversalTime().AddHours(3).ToLongTimeString(),
                    "body",
                    "Conf. Room",
                    "Jean-Michel"
                );
            bool status = await GraphAPILib.Helpers.AddEvent(HttpContext, requestBody);
            if (status)
            {
                return new RedirectResult("/Index");
            }
            else
            {
                //return RedirectToAction("Error", "Home", new { message = "ERROR adding new event", debug = "Error body" });
                return new RedirectResult("/Index");
            }
        }

        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                // Signal OWIN to send an authorization request to Azure
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            if (Request.IsAuthenticated)
            {
                string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    // Get the user's token cache and clear it
                    SessionTokenCache tokenCache = new SessionTokenCache(userId, HttpContext);
                    tokenCache.Clear();
                }
            }
            // Send an OpenID Connect sign-out request. 
            HttpContext.GetOwinContext().Authentication.SignOut(
                CookieAuthenticationDefaults.AuthenticationType);
            Response.Redirect("/");
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
    }
}