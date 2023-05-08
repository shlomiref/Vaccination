namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VaccinationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientVaccinations", "NextVaccinaDate", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientVaccinations", "NextVaccinaDate");
        }
    }
}
