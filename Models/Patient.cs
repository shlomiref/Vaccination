using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppForVaccine.Models
{
    public class Patient
    {

        public int PatientId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
        public string UniqueId { get; set; }
        public string PatientNumber { get; set; }

    }
    public class PatientModel
    {
       
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UniqueId { get; set; }
        public string PatientNumber { get; set; }
    }

    public enum Vaccinated
    {
        Yes,
        No
    }
    
    public class Vaccination
    {
        [Key]
        public int vaccineId { get; set; }
        public string VaccineName { get; set; }
        public string Manufacturer { get; set; }
        public string ValidityOfVaccine { get; set; }
        public int? Month { get; set; }
    }
    public class VaccinationModel
    {
        
        public int vaccineId { get; set; }
        public string VaccineName { get; set; }
        public string Manufacturer { get; set; }
        public string ValidityOfVaccine { get; set; }
         public int Month { get; set; }

        public Vaccination Edit(Vaccination entity, VaccinationModel model)
        {
            entity.vaccineId = model.vaccineId;
            entity.VaccineName = model.VaccineName;
            entity.ValidityOfVaccine = model.ValidityOfVaccine;
            entity.Manufacturer = model.Manufacturer;
            entity.Month = model.Month;
            return entity;
        }
    }


    public class Clinic
    {
        [Key]
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
    }
    public enum UserType
    {      
        Therapist,
        Doctor,
        Admin
    }


}