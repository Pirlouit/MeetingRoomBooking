using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingRoomBooking.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string RoomName { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}