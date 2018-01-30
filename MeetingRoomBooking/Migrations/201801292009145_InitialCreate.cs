namespace MeetingRoomBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BookingName = c.String(),
                        OwnerName = c.String(),
                        BookingDay = c.DateTime(nullable: false),
                        BookingStart = c.DateTime(nullable: false),
                        BookingEnd = c.DateTime(nullable: false),
                        Room_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Rooms", t => t.Room_ID)
                .Index(t => t.Room_ID);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoomName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "Room_ID", "dbo.Rooms");
            DropIndex("dbo.Bookings", new[] { "Room_ID" });
            DropTable("dbo.Rooms");
            DropTable("dbo.Bookings");
        }
    }
}
