//using IoMMobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NFCZavrsniMobile.Data
{
    class CrudApi
    {
        BearerToken Token;

        /// <summary>
        /// 
        /// </summary>
        public CrudApi()
        {
        }
        public CrudApi(BearerToken token)
        {
            Token = token;
        }

        public async Task<bool> PostAsync<T>(Uri url, object o, bool first = false) where T : class
        {
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(o as T);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            if (!first)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.Access_token);
            }
            response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<R> PostAsync<T, R>(Uri url, object o, bool first = false) where T : class where R : class
        {
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(o as T);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            if (!first)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.Access_token);
            }
            response = await client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadAsStringAsync();
            var ret = JsonConvert.DeserializeObject<R>(result);
            return ret;
        }

        public async Task<bool> PutAsync<T>(Uri url, object o, bool first = false) where T : class
        {
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(o as T);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            if (!first)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.Access_token);
            }
            response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
