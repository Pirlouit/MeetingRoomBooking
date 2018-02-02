using MeetingRoomBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MeetingRoomBooking.Controllers
{
    public class RoomAPIController : ApiController
    {
        MyDbContext db = new MyDbContext();
        // GET: api/RoomAPI
        public IEnumerable<Room> Get()
        {
            return db.Rooms.ToList<Models.Room>();
        }

        // GET: api/RoomAPI/5
        public Room Get(int id)
        {
            return db.Rooms.Find(id);
        }

        // POST: api/RoomAPI
        public void Post(Room room)
        {
            db.Rooms.Add(room);
            db.SaveChanges();
        }

        // PUT: api/RoomAPI/5
        public void Put(Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(room).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataMisalignedException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
        }

        // DELETE: api/RoomAPI/5
        public void Delete(int id)
        {
            Room room = new Room() { RoomID = id };
            db.Rooms.Attach(room);
            db.Rooms.Remove(room);
            db.SaveChanges();
        }
    }
}
