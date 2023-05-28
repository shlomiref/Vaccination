using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.EntityFramework;

namespace AppForVaccine.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }       
        public UserType UserTypes { get; set; }
        public bool Status { get; set; } = true;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        

        public DbSet<SmsCode> SmsCodes { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Therapist> Therapists { get; set; }      
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<PatientVaccination> PatientVaccinations { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<CityList> CityLists { get; set; }
       

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}