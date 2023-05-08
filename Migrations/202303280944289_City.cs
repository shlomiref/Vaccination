namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class City : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CityLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CityLists");
        }
    }
}
