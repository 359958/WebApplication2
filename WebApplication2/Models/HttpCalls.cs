using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace WebApplication2.Models
{
    public class HttpCalls
    {
       
        static string webapiurl = System.Configuration.ConfigurationManager.AppSettings["WebapiUrl"].ToString(); 
      
        public static string Httpclientcall(object obj,string action ,string view)
        {
            using (HttpClient client = new HttpClient())
            {
                string stringData = JsonConvert.SerializeObject(obj);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(webapiurl + "/"+ action +"/" + view, contentData).Result;
                var list = response.Content.ReadAsStringAsync().Result;
                return list;
            }
        }

        public static List<T> HttpclientListcall<T>(List<T> lstobj, string action, string view)
        {
            //var list = new List<lstobj>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            response = httpClient.GetAsync(webapiurl + "/" + action + "/" + view).Result;
            response.EnsureSuccessStatusCode();
            string data = response.Content.ReadAsStringAsync().Result;
            var Json = JsonConvert.DeserializeObject<List<T>>(data);
         
            if (!object.Equals(Json, null))
            {
                var JsonList = Json.ToList();
                return JsonList;
            }
            else
            {
                return null;
            }
        }
    }
}