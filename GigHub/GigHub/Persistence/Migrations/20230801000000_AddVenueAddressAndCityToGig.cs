using System;
using System.Data.Entity.Migrations;

namespace GigHub.Persistence.Migrations
{
    public partial class AddVenueAddressAndCityToGig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gigs", "VenueAddress", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Gigs", "City", c => c.String(nullable: false, maxLength: 255));
            CreateTable(
                "dbo.PartnerCities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.PartnerCities");
            DropColumn("dbo.Gigs", "City");
            DropColumn("dbo.Gigs", "VenueAddress");
        }
    }
}
