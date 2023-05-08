namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewColUniqueIdinpaitents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "UniqueId", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "UniqueId");
        }
    }
}
