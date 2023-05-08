using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppForVaccine.Models
{
    public class Therapist
    {
        // [Key]
        public int TherapistId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Clinic { get; set; }

    }

    public class TherapistModel
    {
       [Required]
        public string Email { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public string Address { get; set; }

    }
}