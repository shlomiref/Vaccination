using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppForVaccine.Models
{
    public class SmsEntity
    {
        public string sender { get; set; }
        public string Mobile { get; set; }
        public string content { get; set; }
        //Dictionary fields = new Dictionary();
        //fields.Add("sender", "Sender_ID");
        //fields.Add("mobile", "+447700900123");
        //fields.Add("content", "Hello from Releans API");
    }
}