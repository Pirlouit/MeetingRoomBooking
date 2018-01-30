namespace MeetingRoomBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyForRooms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        BookingName = c.String(),
                        OwnerName = c.String(),
                        BookingDay = c.DateTime(nullable: false),
                        BookingStart = c.Time(nullable: false, precision: 7),
                        BookingEnd = c.Time(nullable: false, precision: 7),
                        RoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomID = c.Int(nullable: false, identity: true),
                        RoomName = c.String(),
                    })
                .PrimaryKey(t => t.RoomID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "RoomId", "dbo.Rooms");
            DropIndex("dbo.Bookings", new[] { "RoomId" });
            DropTable("dbo.Rooms");
            DropTable("dbo.Bookings");
        }
    }
}
