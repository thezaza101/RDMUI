using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using RDMUI.Models;

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
        public static List<Change> GenerateChangeDelta (this Element inElement, Element compare, string changeSetID, string tableId)
        {
            List<Change> output = new List<Change>();
                
            Change c;
            List<string> newKeys = new List<string>();
            foreach (string key in inElement.Values.Keys)
            {
                if(inElement.Values[key].Replace("\n", string.Empty).Replace("\r", string.Empty) != compare.Values[key].Replace("\n", string.Empty).Replace("\r", string.Empty))
                {
                    c = new Change() {
                        Action = ChangeAction.UpdateElement,
                        Active = true,
                        ChangeSetID = changeSetID,
                        ElementID = inElement.ID,
                        ElementName = key,
                        NewValue = compare.Values[key],
                        TableID = tableId
                    };
                    output.Add(c);
                }
            }                
            
            return output;
        }
    }
}

