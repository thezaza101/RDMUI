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

        public string tableId;

        public List<ChangeSet> TableChangeSet;
        public ChangeSet SelectedChangeSet;
        
        public Dictionary<string, string> Values = new Dictionary<string, string>();
        public List<Change> ListChanges;

        public ElementManagerModel()
        {
            client = new RDMClient();
            client.ItemInFocus = false;
        }

        public void OnGet(string focus, string tableId, string action)
        {
            this.tableId = tableId;
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
                        Values = FocusedItem.Values;
                        client.ItemInFocus = true;  
                        Message = (TableChangeSet.Count == 0) ? "Please create a changeset for this table first" : "";
                    }
                    catch (Exception e)
                    {
                        Message = "Something went wrong: " + e.Message;
                        client.ItemInFocus = false;
                    }
                }
            }
        }

        public void OnPost(Element FocusedItem, Dictionary<string, string> Values, ChangeSet SelectedChangeSet, string tableId)
        {
            FocusedItem.Values = Values;
            if (!string.IsNullOrWhiteSpace(SelectedChangeSet.ID))
            {            
                
                if (FocusedItem.ID !="NEW")
                {
                    Element originalElement = client.GetElement(tableId.Trim(), client.GetElementbyIDFromIdentifier(FocusedItem.ID));
                    ListChanges = originalElement.GenerateChangeDelta(FocusedItem,SelectedChangeSet.ID,tableId);
                    Message = "The following changes were made as part of the selected changeset:";

                }
                else
                {
                    Change c = new Change();
                    c.Active = true;
                    c.ChangeSetID = SelectedChangeSet.ID;
                    c.ElementID = FocusedItem.ID;
                    c.NewElementPayload = FocusedItem;
                    c.TableID = tableId;
                    ListChanges = new List<Change>();
                    ListChanges.Add(c);
                }
                if (ListChanges.Count > 0)
                {
                    client.PostItem<Change>(ListChanges);
                }
            }
            else
            {
                Message = "Please select a changeset";
            }
        }
    }
}
