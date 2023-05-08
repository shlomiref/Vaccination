namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scheduledDatew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientVaccinations", "scheduledDate", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientVaccinations", "scheduledDate");
        }
    }
}
