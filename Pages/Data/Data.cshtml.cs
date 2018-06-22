using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDMUI.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RDMUI.Pages
{
    public class DataModel : PageModel
    {
        //https://datatables.net/purchase/index

        public RDMClient client {get;}
        public string Message { get; set; }
        public List<Table> TablesList {get; private set;}        
        
        public Table FocusedItem;

        public List<Release> TableRelease;
        public List<ChangeSet> TableChangeSets;

        public List<Change> FocusedChanges;

        public DataModel()
        {
            client = new RDMClient();
            client.ItemInFocus = false;
        }
        public void OnGet(string focus, string action, string rel, string cs)
        {
            TablesList = client.GetListOf<Table>(true);
            if(!string.IsNullOrWhiteSpace(focus))
            {
                FocusedItem = client.GetItemByID<Table>(focus);
                TableRelease = client.GetReleasesForTable(focus,true);
                TableRelease.Insert(0,new Release(){ID="", Name="Production"});
                TableChangeSets = client.GetChangeSetsForTable(focus,false,string.IsNullOrWhiteSpace(rel));
                client.ItemInFocus = true;

                if (!string.IsNullOrWhiteSpace(rel))
                {
                    if (!string.IsNullOrWhiteSpace(cs))
                    {
                        ChangeSet cSet = TableChangeSets.Single(c => c.ID==cs);
                        FocusedChanges = new List<Change>();
                        foreach (Change change in cSet.Changes)
                        {
                            FocusedChanges.Add(change);
                        }                        
                    }
                    else
                    {
                        List<ChangeSet> releaseChangeSets = TableChangeSets.Where(c => c.ReleaseID==rel).ToList();
                        FocusedChanges = new List<Change>();
                        foreach (ChangeSet cSet in releaseChangeSets)
                        {
                            foreach (Change change in cSet.Changes)
                            {
                                FocusedChanges.Add(change);
                            }
                        }
                    }
                    foreach (Change c in FocusedChanges)
                    {
                        switch (c.Action)
                        {
                            case ChangeAction.AddElement:
                                Element e = JsonConvert.DeserializeObject<Element>(c.NewElementPayload.ToString());                            
                                e.Style = "background-color:#99ffcc";
                                FocusedItem.TableElements.Add(c.ElementID,e);
                                break;
                            
                            case ChangeAction.RemoveElement:
                                FocusedItem.TableElements.Single(s => s.Key == c.ElementID).Value.Style = "background-color:#e6e6e6;text-decoration: line-through;";
                                break;

                            case ChangeAction.UpdateElement:
                                FocusedItem.TableElements.Single(s => s.Key == c.ElementID).Value.Style = "background-color:#ffffe6";
                                FocusedItem.TableElements.Single(s => s.Key == c.ElementID).Value.Values[c.ElementName] = c.NewValue;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}