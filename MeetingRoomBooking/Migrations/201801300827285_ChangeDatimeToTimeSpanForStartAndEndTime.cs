namespace MeetingRoomBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDatimeToTimeSpanForStartAndEndTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "BookingStart", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Bookings", "BookingEnd", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bookings", "BookingEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Bookings", "BookingStart", c => c.DateTime(nullable: false));
        }
    }
}
