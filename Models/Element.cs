using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RDMUI.Models
{
    public class Element
    {
        [Key]
        public string ID {get;set;}
        public Dictionary<string, string> Values = new Dictionary<string, string>();

        public string Style {get; set;}

        public string GetStyle()
        {
            if (string.IsNullOrWhiteSpace(Style))
            {
                return "";
            }
            else
            {
                return Style;
            }
        } 
        
    }
}