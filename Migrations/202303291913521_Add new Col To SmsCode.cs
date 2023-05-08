namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnewColToSmsCode : DbMigration
    {
        public override void Up()
        {
       
            AddColumn("dbo.SmsCodes", "Todday", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SmsCodes", "Todday");
       
        }
    }
}
