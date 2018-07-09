using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDMUI.Models;
using System.ComponentModel.DataAnnotations;
using textdiffcore;
using textdiffcore.DiffOutputGenerators;
using textdiffcore.TextDiffEngine;

namespace RDMUI.Pages
{
    public class ChangeModel : PageModel
    {
        public RDMClient client {get;}        
        public Change FocusedItem;
        public Element ExistingItem;

        public Dictionary<string, string> Values = new Dictionary<string, string>();


        public string Message { get; set; }

        public ChangeModel()
        {
            client = new RDMClient();
            client.ItemInFocus = false;
        }

        public void OnGet(string focus, string action)
        {
            if(!string.IsNullOrWhiteSpace(focus))
            {
                try
                {
                    FocusedItem = client.GetItemByID<Change>(focus);
                    client.ItemInFocus = true;
                    if (FocusedItem.Action != ChangeAction.AddElement)
                    {
                        ExistingItem = client.GetElement(FocusedItem.TableID, client.GetElementbyIDFromIdentifier(FocusedItem.ElementID));
                    } 
                    else 
                    {
                        ExistingItem = Newtonsoft.Json.JsonConvert.DeserializeObject<Element>(FocusedItem.NewElementPayload.ToString());
                    }
                    Values = ExistingItem.Values;

                }
                catch (Exception e)
                {
                    Message = "Something went wrong: " + e.Message;
                    client.ItemInFocus = false;
                }
            }
            else
            {
                Message = "Who are you and how did you get in here? Please go back home!";                
            }
        }
        public string GetComprasonHTML(string tableId, string elementId, string property, string newValue)
        {
            Element old = client.GetElement(tableId,client.GetElementbyIDFromIdentifier(elementId));
            string oldValue = old.Values[property];
            TextDiff diffobj = new TextDiff(new csDiff(), new HTMLDiffOutputGenerator("span", "style", "color:#003300;background-color:#ccff66;","color:#990000;background-color:#ffcc99;text-decoration:line-through;",""));
            return diffobj.GenerateDiffOutput(oldValue,newValue);
        }
        public void OnPost(Change FocusedItem, Dictionary<string, string> Values)
        {
            if (FocusedItem.ID !="NEW")
            {
                Change cItem = client.GetItemByID<Change>(FocusedItem.ID);
                /*if (!(FocusedItem.Active == cItem.Active & FocusedItem.Name==cItem.Name & FocusedItem.ID == cItem.ID & FocusedItem.ChangeSetStatus == cItem.ChangeSetStatus))
                {
                    Message = client.PutItemByID<Change>(FocusedItem.ID, FocusedItem).StatusCode.ToString();
                }
                else
                {
                    Message = "No Action Taken.";
                }*/
            }
            else
            {
                    Message = client.PostItem<Change>(FocusedItem).StatusCode.ToString();                
            }
            OnGet("","");
        }
    }
}
