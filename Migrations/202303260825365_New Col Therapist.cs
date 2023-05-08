namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewColTherapist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientVaccinations", "VaccinationDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.PatientVaccinations", "Therapist", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientVaccinations", "Therapist");
            DropColumn("dbo.PatientVaccinations", "VaccinationDate");
        }
    }
}
