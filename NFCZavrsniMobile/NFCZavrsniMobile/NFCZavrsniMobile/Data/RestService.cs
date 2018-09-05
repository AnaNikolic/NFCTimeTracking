using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Diagnostics;

namespace NFCZavrsniMobile.Data
{
    public class RestService
    {
        private HttpWebRequest ClientBearerPost { get; set; }
        private BearerToken BearerTokenUser { get; set; }
       /* bool SuccessAuth { get; set; }

        public RestService()
        {
            SuccessAuth = false;
        }*/

        public async Task<BearerToken> AuthentificateAsync(Uri url, LogInBody o)
        {
            var client = new HttpClient();
            var content = new StringContent(GetURLstringFromBody(o), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = null;
            response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                 return null;
            }
            var result = await response.Content.ReadAsStringAsync();
            IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "R" };
            BearerToken b = JsonConvert.DeserializeObject<BearerToken>(result, dateTimeConverter);
            return b;
        }
        
        string GetURLstringFromBody(LogInBody o)
        {
            List<string> polje = new List<string>();

            foreach (var property in o.GetType().GetProperties())
            {
                polje.Add((property.Name).ToLower() + "=" + Uri.EscapeDataString(property.GetValue(o).ToString()));
            }
            string s = string.Join("&", polje);
            
            return s;

        }
    }
}

