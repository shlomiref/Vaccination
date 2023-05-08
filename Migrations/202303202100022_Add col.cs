namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientVaccinations", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientVaccinations", "Status");
        }
    }
}
