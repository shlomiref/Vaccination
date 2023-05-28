using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppForVaccine.Models
{
    public class index
    {

        public int therapistNum { get; set; }
        public int cityNum { get; set; }
        public int UserNum { get; set; }
        public int patientNum { get; set; }



    }
    public class MyUserModel
    {
        public string id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public UserType UserTypes { get; set; }
        [Required]
        public bool Status { get; set; }       
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }




        public ApplicationUser Edit(ApplicationUser entity, MyUserModel model)
        {
            entity.Id = model.id;
            entity.Email = model.Email;
            entity.Status = model.Status;
            entity.PhoneNumber = model.PhoneNumber;
            entity.UserTypes = model.UserTypes;
            entity.Name = model.Name;
            entity.UserName = model.Email;

            return entity;
        }
    }

    public class VacinatedModelView
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Vaccinated Vaccination { get; set; }
        public string City { get; set; }
        public string DateTime { get; set; }  
        
        public string PhoneNumber { get; set; }
    }
    public class AppointmentFormViewModel
    {
       

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Time { get; set; }


        [Required]
        public string Detail { get; set; }

        
        [Required]
        public string PatientEmail { get; set; }       

        [Required]    

        public IEnumerable<Therapist> Therapists { get; set; }
        
        public DateTime GetStartDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }


    public class AppointmentViewModel
    {
       
        public string Date { get; set; }       
        public string Time { get; set; }
        public string Detail { get; set; }
        public bool Status { get; set; }
        public string Patient { get; set; }
        public string Therapist { get; set; }
    }
}