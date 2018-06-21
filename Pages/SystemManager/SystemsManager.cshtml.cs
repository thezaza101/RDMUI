using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using RDMUI.Models;

namespace RDMUI.Pages
{
    public class SystemsManagerModel : PageModel
    {
        public RDMClient client {get;}
        public string Message { get; set; }

        public List<RDSystem> ExistingSystems {get;set;}
        public RDSystem FocusedItem;


        public SystemsManagerModel()
        {
            client = new RDMClient();
            client.ItemInFocus = false;
        }

        public void OnGet(string focus, string action)
        {
            ExistingSystems = client.GetListOf<RDSystem>();

            if (action=="A")
            {
                FocusedItem = new RDSystem(){ID="NEW"};
                client.ItemInFocus = true;
            }
            else 
            {
                if(!string.IsNullOrWhiteSpace(focus))
                {
                    try
                    {
                        FocusedItem = ExistingSystems.Single(i => i.ID.Equals(focus.Trim())); 
                        client.ItemInFocus = true;  
                    }
                    catch (Exception e)
                    {
                        Message = "Something went wrong: " + e.Message;
                        client.ItemInFocus = false;
                    }
                }
            }
        }

        public void OnPost(RDSystem FocusedItem)
        {
            if (FocusedItem.ID !="NEW")
            {
                RDSystem cItem = client.GetItemByID<RDSystem>(FocusedItem.ID);
                if (!(FocusedItem.Active == cItem.Active & FocusedItem.Name==cItem.Name & FocusedItem.ID == cItem.ID))
                {
                    Message = client.PutItemByID<RDSystem>(FocusedItem.ID, FocusedItem).StatusCode.ToString();
                }
                else
                {
                    Message = "No Action Taken.";
                }
            }
            else
            {
                    Message = client.PostItem<RDSystem>(FocusedItem).StatusCode.ToString();                
            }
            OnGet("","");            
        }
    }
}
