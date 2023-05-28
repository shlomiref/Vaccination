using AppForVaccine.Models;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(AppForVaccine.Startup))]
namespace AppForVaccine
{

    public partial class Startup
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            //providing database name to save jobs etc
            // GlobalConfiguration.Configuration.UseStorage( new MySqlStorage("localhost;database=VaccineAppdatabase;userid=root;password=cliffosa;"));
            var connectionString = "server=localhost;database=VaccineAppDb;userid=root;password=root";
            GlobalConfiguration.Configuration.UseStorage( new MySqlStorage("server=localhost;database=VaccineAppDb;userid=root;password=root",
            new MySqlStorageOptions
            {
               // TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablesPrefix = "Hangfire"
            }));
           // GlobalConfiguration.Configuration.UseStorage<UseMySqlStorage>("WebAppConnection");

            //basic process to check
            BackgroundJob.Enqueue(() => Console.WriteLine("Getting Started with HangFire!"));
            RecurringJob.AddOrUpdate(() => TaskReminder(), Cron.Weekly);

            //will create hangfire dashboard
            app.UseHangfireDashboard();
            app.UseHangfireServer();

        }

        public void TaskReminder()
        {
            DateTime filterD = DateTime.Now.AddDays(7);

            var PatientData = _db.PatientVaccinations.Where(x => x.Status == false && x.NextVaccinaDate <= filterD).ToList();
            var vReminderlist = new List<VaccinationReminderModel>();
            foreach (var item in PatientData)
            {
                var phn = _db.Patients.Where(x => x.PatientId == item.PatientId).FirstOrDefault();
                var snd = new SmsEntity()
                {
                    content = item.VaccineName+ "זוהי תזכורת עליכם לבצע את החיסון" + phn.LastName + phn.FirstName+ "שלום"  ,
                    Mobile = phn.Phone,
                    sender = "Vaccine Team"
                };

                SmsSenderPoint smsSenderPoint = new SmsSenderPoint();
                smsSenderPoint.GetAPIReponse(snd);
                var Patv = _db.PatientVaccinations.Where(x => x.PatientVaccinatedId == item.PatientVaccinatedId).FirstOrDefault();
                Patv.ReminderSent = true;
                _db.Entry(Patv).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
               
                user.UserName = "admin@gmail.com"; // change the email
                user.Email = "admin@gmail.com";  //change email
                user.Name ="Shlomi Refaelof"; //change name
                user.UserTypes = UserType.Admin;
                user.PhoneNumber="0503892656"; //change phone number
                user.Status = true;


                string userPWD = "Password123@";   //change password

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin 

                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }
            
            // creating Creating Manager role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

            }

            if (!roleManager.RoleExists("Therapist"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Therapist";
                roleManager.Create(role);

            }

        }

    }
}