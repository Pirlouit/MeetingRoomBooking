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
            //return db.Bookings.ToList().se;
            return null;
        }

        // GET: api/BookingAPI/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/BookingAPI
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/BookingAPI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BookingAPI/5
        public void Delete(int id)
        {
        }
    }
}
