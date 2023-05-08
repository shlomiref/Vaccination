namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "PatientNumber", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "PatientNumber");
        }
    }
}
