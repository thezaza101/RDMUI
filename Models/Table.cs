using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RDMUI.Models
{
    public class Table
    {
        [Key]
        public string ID {get;set;}
        public string SystemID {get;set;}
        public Dictionary<string, string> TableProperties = new Dictionary<string, string>(); 
        public Dictionary<string, Element> TableElements = new Dictionary<string, Element>(); 
    }
}