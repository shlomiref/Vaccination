using AppForVaccine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace AppForVaccine
{
    public  class SmsSenderPoint
    {


        public async Task GetAPIReponse(SmsEntity snd)
        {
            try
            {
                var fields = new Dictionary<string, string>();
                fields.Add("sender", "vaccination");
                fields.Add("mobile", snd.Mobile);
                fields.Add("content", snd.content);

                var serializer = new JavaScriptSerializer();
                var serializedResult = serializer.Serialize(fields);
                var data = new StringContent(serializedResult, Encoding.UTF8, "application/json");

                var httpclient = new HttpClient();
                httpclient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJpZCI6IjMyMDQ1ZWI4LTQxOTEtNDY0NC1hYWZiLWQwMmUzZWQwM2RhYSIsImlhdCI6MTY4MDExNzkwNiwiaXNzIjoxNTk1Mn0.HPlXZt5jHxxZHiU1yR0luMh6vBMP34fay06BCTJtHFw");
                var response = await httpclient.PostAsync("https://api.releans.com/v2/message", data);
                string page = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {

                throw;
            }
           

           // return page;
        }


    }
}