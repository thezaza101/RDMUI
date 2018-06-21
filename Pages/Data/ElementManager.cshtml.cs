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
    public class ElementManagerModel : PageModel
    {
        public RDMClient client {get;}
        public string Message { get; set; }

        public Element FocusedItem;

        public List<ChangeSet> TableChangeSet;
        public ChangeSet SelectedChangeSet;
        


        public ElementManagerModel()
        {
            client = new RDMClient();
            client.ItemInFocus = false;
        }

        public void OnGet(string focus, string tableId, string action)
        {
            if (action=="A")
            {
                FocusedItem = new Element(){ID="NEW"};
                client.ItemInFocus = true;
            }
            else 
            {
                if(!string.IsNullOrWhiteSpace(focus))
                {
                    try
                    {
                        FocusedItem = client.GetElement(tableId.Trim(), focus.Trim());
                        TableChangeSet = client.GetChangeSetsForTable(tableId,true);
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

        public void OnPost(Element FocusedItem, ChangeSet SelectedChangeSet)
        {
            KeyValuePair<string,string> dx =  FocusedItem.Values.ElementAt(0);
            /*
            if (FocusedItem.ID !="NEW")
            {
                //TODO Code to create List<Changes> and post
            }
            else
            {
                //TODO Code to create/delete item              
            }
            //TODO Redirect to data page with table in focus
             */          
        }
    }
}
