using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppForVaccine.Models
{
    public class PatientVaccination
    {
        [Key]
        public int PatientVaccinatedId { get; set; }
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VaccineName { get; set; }
        public bool Status { get; set; }
        public Nullable<DateTime> scheduledDate { get; set; }
        public int? NextVaccination { get; set; }
        public string UniqueId { get; set; }
        public Nullable<DateTime> VaccinationDate { get; set; }
        public string Therapist { get; set; }
        public string City { get; set; }
        public Nullable<DateTime> NextVaccinaDate { get; set; }
        public bool? ReminderSent { get; set; }
        public string PatientNumber { get; set; }

        public List<PatientVaccinationModelList> PatList { get; set; }
    }

    public class PatientVaccinationModel
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UniqueId { get; set; }
        public List<PatientVaccinationModelList> PatList{ get; set; }
       
    }

    public class VaccinationReminderModel
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PatientVaccinatedId { get; set; }
        public string VaccineName { get; set; }
        public bool Status { get; set; }
        public bool ReminderSent { get; set; }
        public string scheduledDate { get; set; }
        public string PatientNumber { get; set; }

    }

    public class ReportModel
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VaccineName { get; set; }
        public bool Status { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string PatientNumber { get; set; }
    }


    public class PatientVaccinationModelList
    {
        [Key]
        public int ModelId { get; set; }
        public int PatientVaccinatedId { get; set; }
        public string VaccineName { get; set; }
        public bool Status { get; set; }
        public string scheduledDate { get; set; }
        public string NextVaccineDate { get; set; }
        public string VaccinatedDate { get; set; }
        public string Therapist { get; set; }
    }


   
}