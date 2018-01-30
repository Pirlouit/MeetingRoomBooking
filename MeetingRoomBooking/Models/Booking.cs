using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MeetingRoomBooking.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        
        [Display(Name ="Nom de la reservation")]
        public string BookingName { get; set; }

        [Display(Name = "Propriétaire de la réservation")]
        public string OwnerName { get; set; }

        [Display(Name = "Jour de la réservation")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingDay { get; set; }

        [Display(Name = "Début de la réservation")]
        public TimeSpan BookingStart { get; set; }

        [Display(Name = "Fin de la réservation")]
        public TimeSpan BookingEnd { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}