namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewColUniqueId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientVaccinations", "UniqueId", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientVaccinations", "UniqueId");
        }
    }
}
