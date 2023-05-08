namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewColMonth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientVaccinations", "NextVaccination", c => c.Int());
            AddColumn("dbo.Vaccinations", "Month", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vaccinations", "Month");
            DropColumn("dbo.PatientVaccinations", "NextVaccination");
        }
    }
}
