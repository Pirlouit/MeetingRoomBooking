using MeetingRoomBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MeetingRoomBooking.Controllers
{
    public class BookingAPIController : ApiController
    {
        MyDbContext db = new MyDbContext();
        
        // GET: api/BookingAPI
        public IEnumerable<Models.Booking> Get()
        {
            return db.Bookings.ToList<Models.Booking>();
        }

        // GET: api/BookingAPI/5
        public Booking Get(int id)
        {
            return db.Bookings.Find(id);
        }

        // POST: api/BookingAPI
        public void Post(Booking booking)
        {
            db.Bookings.Add(booking);
            db.SaveChanges();
        }

        // PUT: api/BookingAPI/5
        public void Put(Booking booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(booking).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataMisalignedException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
        }

        // DELETE: api/BookingAPI/5
        public void Delete(int id)
        {
            Booking booking = new Booking() { BookingID = id };
            db.Bookings.Attach(booking);
            db.Bookings.Remove(booking);
            db.SaveChanges();
        }
        [ActionName("GetBookingByDay")]
        [HttpPost]
        public IEnumerable<Models.Booking> GetBookingByDay([FromBody] string bookingDay)
        { 
            if (bookingDay == null)
                return null;

            DateTime dt = DateTime.Parse(bookingDay);
            return db.Bookings.Where(c => c.BookingDay == dt).ToList<Booking>();
        }
        [ActionName("GetBookingByRoomId")]
        [HttpPost]
        public IEnumerable<Models.Booking> GetBookingByRoomId([FromBody] int roomId)
        {
            return db.Bookings.Where(c => c.RoomId == roomId);
        }
    }
}
