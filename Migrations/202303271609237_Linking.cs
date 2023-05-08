namespace AppForVaccine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Linking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientVaccinationModelLists",
                c => new
                    {
                        ModelId = c.Int(nullable: false, identity: true),
                        PatientVaccinatedId = c.Int(nullable: false),
                        VaccineName = c.String(unicode: false),
                        Status = c.Boolean(nullable: false),
                        scheduledDate = c.String(unicode: false),
                        NextVaccineDate = c.String(unicode: false),
                        VaccinatedDate = c.String(unicode: false),
                        Therapist = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ModelId)
                .ForeignKey("dbo.PatientVaccinations", t => t.PatientVaccinatedId, cascadeDelete: true)
                .Index(t => t.PatientVaccinatedId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientVaccinationModelLists", "PatientVaccinatedId", "dbo.PatientVaccinations");
            DropIndex("dbo.PatientVaccinationModelLists", new[] { "PatientVaccinatedId" });
            DropTable("dbo.PatientVaccinationModelLists");
        }
    }
}
