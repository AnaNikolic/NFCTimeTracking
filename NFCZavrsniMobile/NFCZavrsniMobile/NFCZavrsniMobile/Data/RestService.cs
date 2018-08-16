using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using IoMMobileApp.Models;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Diagnostics;

namespace NFCZavrsniMobile.Data
{
    /// <summary>
    /// Klasa RestServis koristi se za komunikaciju s rest servisom tokom rada aplikacije
    /// U konstruktoru klase se postavlja vrijednost uspjesnosti autentifikacije na false te se mjenaj tokom rada apliakcije
    /// </summary>

    public class RestService
    {
        private HttpWebRequest ClientBearerPost { get; set; }
        private BearerToken BearerTokenUser { get; set; }
        bool SuccessAuth { get; set; }

        public RestService()
        {
            SuccessAuth = false;
        }

        /// <summary>
        /// Metoda Autentificiraj vraća true ako je autentifikacija uspješna a false ako je autentifikacija neuspješna
        /// </summary>
        /// 
        /*
        public async Task<bool> InitiateLogin(string phoneID, string phoneNumberInput)
        {
            InitiateLogInBody body = new InitiateLogInBody(phoneID, phoneNumberInput);
            Uri restUri = new Uri(Constants.RestURLInitiateLogin);

            try
            {
                ClientBearerPost = (HttpWebRequest)WebRequest.Create(Constants.RestURLInitiateLogin);
                body = new InitiateLogInBody(phoneID, phoneNumberInput);
                ClientBearerPost.Method = "POST";
                ClientBearerPost.ContentType = "application/x-www-form-urlencoded";
                //ClientBearerPost.ContentType = "application/raw";

                var data = Encoding.ASCII.GetBytes(GetURLstringFromBody(body));
                ClientBearerPost.ContentLength = data.Length;
                using (var stream = ClientBearerPost.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)ClientBearerPost.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                Debug.WriteLine(@"ERROR {0}", ex.Message);
                throw;
            }
        }*/


       /* losiiiiiiiiiiiii
    public async Task<BearerToken> AuthentificateAsync(Uri restUri, LogInBody tijelo)
    {
        //Uri restUri = new Uri(Constants.RestURLBearerToken);

        try
        {
           ClientBearerPost = (HttpWebRequest)WebRequest.Create(Constants.RestURLBearerToken);
            //ClientBearerPost = (HttpWebRequest)WebRequest.Create(restUri);
            ClientBearerPost.Method = "POST";
            ClientBearerPost.ContentType = "application/x-www-form-urlencoded";

            var data = Encoding.ASCII.GetBytes(GetURLstringFromBody(tijelo));
            ClientBearerPost.ContentLength = data.Length;

            using (var stream = ClientBearerPost.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)ClientBearerPost.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "R" };
            BearerToken b = JsonConvert.DeserializeObject<BearerToken>(responseString, dateTimeConverter);
            return b;
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine(@"ERROR {0}", ex.Message);
            throw;
        }

    }
    */
    
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

