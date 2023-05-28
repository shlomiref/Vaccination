using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
namespace AppForVaccine
{
    public class Sms019
    {
        public static void Main()
        {

            System.Environment.SetEnvironmentVariable("TWILIO_ACCOUNT_SID", "ACed98ad93dd2d2469ae4527f1df18ce4d");
            System.Environment.SetEnvironmentVariable("TWILIO_AUTH_TOKEN", "7bbab09f3f6c35192515a96d66603f6c");
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            TwilioClient.Init(accountSid, authToken);

        }

        public void Validation(string Code,string phn) {

            
            var message = MessageResource.Create(
                body: Code +  "  שלום, קוד האימות שלך הוא",
                 from: new Twilio.Types.PhoneNumber("+13203226247"),
                  to: new Twilio.Types.PhoneNumber(phn)


              );
                


        }

    }
}