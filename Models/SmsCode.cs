using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppForVaccine.Models
{
    public class SmsCode
    {
        //[Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public Nullable<DateTime> Todday { get; set; }
    }
}