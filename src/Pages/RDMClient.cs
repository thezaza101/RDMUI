using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using RDMUI.Models;
using Newtonsoft.Json;

namespace RDMUI.Pages
{
    public class RDMClient
    {
        public HttpClient HTTPClient {get;} = Helpers.NewHTTPClinet();
        public static Dictionary<Type, string> EP {get;}
        public bool ItemInFocus {get; set;}

        static RDMClient()
        {
            EP = new Dictionary<Type, string>();
            EP.Add(typeof(RDSystem),"System");
            EP.Add(typeof(Release),"Release");
            EP.Add(typeof(ChangeSet),"ChangeSet");
            EP.Add(typeof(Change),"Change");
            EP.Add(typeof(Table),"Data");
            EP.Add(typeof(Element),"Data");
        }
        public List<T> GetListOf<T>(bool listOnly = false)
        {
            List<T> output;
            try
            {
                string attr = listOnly ? "?list=true" : "";
                string data = HTTPClient.GetAsync(GetEndPoint<T>()+attr).Result.Content.ReadAsStringAsync().Result;
                output = JsonConvert.DeserializeObject<List<T>>(data);
            }
            catch
            {
                output = new List<T>();
            }

            return output;
        }
        public T GetItemByID<T>(string id)
        {
            T output;
            string data = HTTPClient.GetAsync(GetEndPoint<T>(id)).Result.Content.ReadAsStringAsync().Result;
            output = JsonConvert.DeserializeObject<T>(data);
            return output;
        }

        public Element GetElement(string tableID, string elementId)
        {
            Element output;            
            string data = HTTPClient.GetAsync(GetEndPoint<Table>(tableID)+"/"+elementId).Result.Content.ReadAsStringAsync().Result;
            output = JsonConvert.DeserializeObject<Element>(data);
            return output;
        }

        public List<Release> GetReleasesForTable(string tableID, bool listOnly = false)
        {
            List<Release> output;
            string attr = listOnly ? "?tableID="+tableID+"&list=true" : "?tableID="+tableID;
            string data = HTTPClient.GetAsync(GetEndPoint<Release>()+attr).Result.Content.ReadAsStringAsync().Result;
            output = JsonConvert.DeserializeObject<List<Release>>(data);
            return output;            
        }
        public List<ChangeSet> GetChangeSetsForTable(string tableID, bool ignoreChanges = false, bool listOnly = false)
        {
            List<ChangeSet> output;
            string attr = ignoreChanges ? "?tableID="+tableID+"&ignoreChanges=true" : "?tableID="+tableID;
            attr = listOnly ? attr + "&list=true" : attr;
            string data = HTTPClient.GetAsync(GetEndPoint<ChangeSet>()+attr).Result.Content.ReadAsStringAsync().Result;
            output = JsonConvert.DeserializeObject<List<ChangeSet>>(data);
            return output;
        }
        

        public HttpResponseMessage PutItemByID<T>(string id, object content)
        {
            return HTTPClient.PutAsync(GetEndPoint<T>(id),Helpers.NewStringContent(content)).Result;
        }

        public HttpResponseMessage PostItem<T>(object content)
        {
            return HTTPClient.PostAsync(GetEndPoint<T>(),Helpers.NewStringContent(content)).Result;
        }

        private string GetEndPoint<T>()
        {
            return EP[typeof(T)];
        }

        private string GetEndPoint<T>(string id)
        {
            return EP[typeof(T)] + "/" + id;
        }
        public string GetElementbyIDFromIdentifier(string identifier)
        {
            return identifier.Substring(identifier.LastIndexOf('/')+1);
        }
    }
}