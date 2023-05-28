using AppForVaccine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AppForVaccine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text; // for class Encoding
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace AppForVaccine
{
    public  class SmsSenderPoint
    {


        public async Task GetAPIReponse(SmsEntity snd)
        {
            System.Environment.SetEnvironmentVariable("TWILIO_ACCOUNT_SID", "ACed98ad93dd2d2469ae4527f1df18ce4d");
            System.Environment.SetEnvironmentVariable("TWILIO_AUTH_TOKEN", "2e77daf10a43ca682514dccdd2aa6ea4");
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            TwilioClient.Init(accountSid, authToken);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            


              
            try
            {
                //var message = MessageResource.Create(
               // body: snd.content,
               // from: new Twilio.Types.PhoneNumber("+13203226247"),
               // to: new Twilio.Types.PhoneNumber(snd.Mobile));
          

                var fields = new Dictionary<string, string>();
                fields.Add("sender", "vaccination");
                fields.Add("mobile", snd.Mobile);
                fields.Add("content", snd.content);

                var serializer = new JavaScriptSerializer();
                var serializedResult = serializer.Serialize(fields);
                var data = new StringContent(serializedResult, Encoding.UTF8, "application/json");

                //var httpclient = new HttpClient();
                //httpclient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJpZCI6IjMyMDQ1ZWI4LTQxOTEtNDY0NC1hYWZiLWQwMmUzZWQwM2RhYSIsImlhdCI6MTY4MDExNzkwNiwiaXNzIjoxNTk1Mn0.HPlXZt5jHxxZHiU1yR0luMh6vBMP34fay06BCTJtHFw");
                //var response = await httpclient.PostAsync("https://api.releans.com/v2/message", data);
                //var response = message.Sid;
                //string page = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {

                throw;
            }
           

           // return page;
        }


    }
}