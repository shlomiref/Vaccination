namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cityrequiredtrue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CityLists", "City", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CityLists", "City", c => c.String(unicode: false));
        }
    }
}
