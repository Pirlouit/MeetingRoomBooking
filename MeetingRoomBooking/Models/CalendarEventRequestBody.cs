using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingRoomBooking.Models
{
    public class BaseEventRequestBody
    {
        public BaseEventRequestBody(string subject, string dateStart, string dateEnd, string body, string location, string organizerName = "")
        {
            this.subject = subject;
            this.start = new EventTime(dateStart);
            this.end = new EventTime(dateEnd);
            this.body = new Body(body);
            this.location = new Location(location);
            this.organizer = new Organizer(new EmailAddress("", organizerName));
        }
        public string subject { get; set; }
        public Organizer organizer { get; set; }
        public Body body { get; set; }
        public Location location { get; set; }
        public EventTime start { get; set; }
        public EventTime end { get; set; }
        public int reminderMinutesBeforeStart { get; set; }
        public bool isReminderOn { get; set; }
        public bool isOrganizer { get; set; }
    }

    public class CalendarEventRequestBody
    {
        public CalendarEventRequestBody(string subject, string dateStart, string dateEnd, string body, string location, string organizerName)
        {
            this.subject = subject;
            this.start = new EventTime(dateStart);
            this.end = new EventTime(dateEnd);
            this.body = new Body(body);
            this.location = new Location(location);
            this.organizer = new Organizer(new EmailAddress("", organizerName));
        }
        public string subject { get; set; }
        public Organizer organizer { get; set; }
        public Body body { get; set; }
        public Attendee[] attendees { get; set; }
        public Location location { get; set; }
        public EventTime start { get; set; }
        public EventTime end { get; set; }
        public int reminderMinutesBeforeStart { get; set; }
        public bool isReminderOn { get; set; }
        public bool isOrganizer { get; set; }
    }

    public class EventTime
    {
        public EventTime(string dateTime)
        {
            this.timeZone = "UTC";
            this.dateTime = dateTime;
        }

        public string dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class Location
    {
        public Location(string displayName)
        {
            this.displayName = displayName;
        }

        public string displayName { get; set; }
    }

    public class Attendee
    {
        public Attendee(EmailAddress emailAddress)
        {
            this.emailAddress = emailAddress;
            this.type = "required";
        }

        public EmailAddress emailAddress { get; set; }
        public string type { get; set; }
    }

    public class Body
    {
        public Body(string content)
        {
            this.content = content;
        }

        public string content { get; set; }
    }

    public class Organizer
    {
        public Organizer(EmailAddress emailAddress)
        {
            this.emailAddress = emailAddress;
        }

        public EmailAddress emailAddress { get; set; }
    }

    public class EmailAddress
    {
        public EmailAddress(string address, string name)
        {
            this.address = address;
            this.name = name;
        }

        public string address { get; set; }
        public string name { get; set; }
    }

    /*
     * json = JsonConvert.SerializeObject(new CalendarEventRequestBody
            {
                subject = "Model event test",
                organizer = new Organizer
                {
                    emailAddress = new Models.EmailAddress
                    {
                        address = "thenkod@hotmail.com",
                        name = "Jean Michel"
                    }
                },
                start = new EventTime
                {
                    dateTime = "2018-02-02T14:30:00.529Z",
                    timeZone = "UTC"
                },
                end = new EventTime
                {
                    dateTime = "2018-02-02T17:00:00.529Z",
                    timeZone = "UTC"
                }
            });
     */
}