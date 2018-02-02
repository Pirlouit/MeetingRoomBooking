using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MeetingRoomBooking.Models
{
    public class Room
    {
        public int RoomID { get; set; }

        [Display(Name = "Nom de la salle")]
        public string RoomName { get; set; }
        [NotMapped]
        public ICollection<Booking> Bookings { get; set; }
    }
}