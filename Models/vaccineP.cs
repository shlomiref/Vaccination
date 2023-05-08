using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppForVaccine.Models
{

    public class vaccineP
    {
        public int PatientId { get; set; }
        public int PatientVaccinatedId { get; set; }
        [Required]
        public bool Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string vaccineName { get; set; }
        public string Therapist { get; set; }
        public string PatientNumber { get; set; }
        //[Required]
        //public string City { get; set; }


        public PatientVaccination EditModel(PatientVaccination entity, vaccineP model)
        {
            entity.PatientId = model.PatientId;
            entity.PatientVaccinatedId = model.PatientVaccinatedId;
            entity.Status = model.Status;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Therapist = model.Therapist;
            entity.VaccinationDate = DateTime.Now;
            //entity.City = model.City;

            return entity;
        }
    }
   

}