using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDMUI.Models;
using System.ComponentModel.DataAnnotations;

namespace RDMUI.Pages
{
    public class DataModel : PageModel
    {
        //https://datatables.net/purchase/index

        public RDMClient client {get;}
        public string Message { get; set; }
        public List<Table> TablesList {get; private set;}        
        
        public Table FocusedItem;

        public DataModel()
        {
            client = new RDMClient();
            client.ItemInFocus = false;
        }
        public void OnGet(string focus, string action)
        {
            TablesList = client.GetListOf<Table>(true);
            if(!string.IsNullOrWhiteSpace(focus))
            {
                FocusedItem = client.GetItemByID<Table>(focus);
                client.ItemInFocus = true;
            }            
        }
    }
}