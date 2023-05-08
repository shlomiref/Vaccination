namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReminderSent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientVaccinations", "ReminderSent", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientVaccinations", "ReminderSent");
        }
    }
}
