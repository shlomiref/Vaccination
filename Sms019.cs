using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text; // for class Encoding
using System.IO;

namespace AppForVaccine
{
    public class Sms019
    {
        static void Main(string phone, string code)
        {

            String url = "https://019sms.co.il/api";
            String xml = "<?xml version='1.0' encoding='UTF8' Authorization=' Bearer eyJ0eXAiOiJqd3QiLCJhbGciOiJIUzI1NiJ9.eyJmaXJzdF9rZXkiOiI1MTE4OCIsInNlY29uZF9rZXkiOiIzMTYwOTA2IiwiaXNzdWVkQXQiOiIxNy0wNC0yMDIzIDA3OjQzOjE5IiwidHRsIjo2MzA3MjAwMH0.2UW2NhaavdhJE9HZlf4VWRbzphGVKlhfc8hGUF7sXMs'?><sms><user><username>Shlomiref</username></user>< source > Vaccination Team </ source >   < destinations >< phone >" + phone+ "</ phone >< phone > " + phone + " </ phone ></ destinations >< message > test code " + code + " </ message ></ sms > ";
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            webRequest.ContentType = "application/xml";
            webRequest.ContentLength = (long)bytes.Length;
            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            
 requestStream.Close();
            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            string result = streamReader.ReadToEnd();
            streamReader.Close(); responseStream.Close(); response.Close();
            Console.WriteLine(result);
        }
    }
}