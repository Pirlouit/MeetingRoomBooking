﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingRoomBooking.Models
{
    public class Booking
    {
        public int ID { get; set; }
        
        public string BookingName { get; set; }

        public string OwnerName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingDay { get; set; }

        public TimeSpan BookingStart { get; set; }
        
        public TimeSpan BookingEnd { get; set; }

        public Room Room { get; set; }
    }
}