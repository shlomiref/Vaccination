namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientVaccinations", "PatientNumber", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientVaccinations", "PatientNumber");
        }
    }
}
