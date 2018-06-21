using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RDMUI.Pages
{
    public static class Helpers
    {
        public static HttpClient NewHTTPClinet()
        {
            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("DataServer"));
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            return _client;
        }

        public static StringContent NewStringContent(object content)
        {
            return NewStringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content));
        }
        public static StringContent NewStringContent(string content)
        {
            StringContent _content = new StringContent(content);
            _content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return _content;
        }
        public static string GetStringFromResponce(HttpResponseMessage response)
        {
            string output ="";
            output = response.Content.ReadAsStringAsync().Result;
            return output;
        }
    }
}

