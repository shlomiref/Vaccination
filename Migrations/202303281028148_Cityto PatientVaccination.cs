namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CitytoPatientVaccination : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientVaccinations", "City", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientVaccinations", "City");
        }
    }
}
