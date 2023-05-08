using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppForVaccine.Models
{
    public class Appointment
    {
        // [Key]
        public int AppointmentId { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Time  { get; set; }
        public string Detail { get; set; }
        public bool Status { get; set; }
        public string PatientName{ get; set; }
        public string TherapistName { get; set; }
    }
}