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
    public class ChangeSetsModel : PageModel
    {
        public RDMClient client {get;}        
        public List<ChangeSet> ExistingChangeSets {get;set;} 
        public List<Release> ExistingReleases {get;set;}

        public ChangeSet FocusedItem;
        public List<Change> FocusedItemChanges {get; set;}

        public string Message { get; set; }

        public ChangeSetsModel()
        {
            client = new RDMClient();
            client.ItemInFocus = false;
        }

        public void OnGet(string focus, string action)
        {
            ExistingChangeSets = client.GetListOf<ChangeSet>(true);

            if (action=="A")
            {
                FocusedItem = new ChangeSet(){ID="NEW"};
                client.ItemInFocus = true;
                ExistingReleases = client.GetListOf<Release>();
            }
            else 
            {
                if(!string.IsNullOrWhiteSpace(focus))
                {
                    try
                    {
                        FocusedItem = client.GetItemByID<ChangeSet>(focus);
                        FocusedItemChanges = FocusedItem.Changes;
                        ExistingReleases = new List<Release>();
                        ExistingReleases.Add(client.GetItemByID<Release>(FocusedItem.ReleaseID));
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
        public string GetReleaseName(string id)
        {
            return client.GetItemByID<Release>(id).Name;
        }
        public string GetComprasonHTML(string tableId, string elementId, string property, string newValue)
        {
            Element old = client.GetElement(tableId,client.GetElementbyIDFromIdentifier(elementId));
            string oldValue = old.Values[property];
            TextDiff diffobj = new TextDiff(new csDiff(), new HTMLDiffOutputGenerator("span", "style", "color:#003300;background-color:#ccff66;","color:#990000;background-color:#ffcc99;text-decoration:line-through;",""));
            return diffobj.GenerateDiffOutput(oldValue,newValue);
        }
        public void OnPost(ChangeSet FocusedItem)
        {
            if (FocusedItem.ID !="NEW")
            {
                ChangeSet cItem = client.GetItemByID<ChangeSet>(FocusedItem.ID);
                if (!(FocusedItem.Active == cItem.Active & FocusedItem.Name==cItem.Name & FocusedItem.ID == cItem.ID & FocusedItem.ChangeSetStatus == cItem.ChangeSetStatus))
                {
                    Message = client.PutItemByID<ChangeSet>(FocusedItem.ID, FocusedItem).StatusCode.ToString();
                }
                else
                {
                    Message = "No Action Taken.";
                }
            }
            else
            {
                    Message = client.PostItem<ChangeSet>(FocusedItem).StatusCode.ToString();                
            }
            OnGet("","");
        }
    }
}
