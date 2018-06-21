using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RDMUI.Pages
{
    
    public class IndexModel : PageModel
    {
        RDMClient client;
        public List<string> TableList {get; set;}
        public bool ServerConnectionAlive {get; set;} = false;
        public IndexModel()
        {   
            client = new RDMClient();
        }

        public void OnGet()
        {
            SetServerStatus();

        }

        private void SetServerStatus()
        {
            string data = "";
            try
            {
                HttpResponseMessage response = client.HTTPClient.GetAsync("Status").Result;
                data = response.Content.ReadAsStringAsync().Result;
                ViewData["ServerStatus"] = "Connection estibilished";
                ServerConnectionAlive = true;
            }
            catch
            {

                ViewData["ServerStatus"] = "Connection failed";
            }
            if (ServerConnectionAlive) 
            {
                JObject responseObject = JsonConvert.DeserializeObject<JObject>(data);
                ViewData["systemCount"] = (string)responseObject["systemCount"];
                ViewData["releaseCount"] = (string)responseObject["releaseCount"];
                ViewData["changeSetCount"] = (string)responseObject["changeSetCount"];
                ViewData["tableCount"] = (string)responseObject["tableCount"];
                var tbls = (JArray)responseObject["tableList"];
                List<JToken> tbls1 = tbls.ToList();
                TableList = new List<string>();
                tbls1.ForEach(e => TableList.Add((string)e));
                
            }
            else
            {

            }

        }
    }
}
