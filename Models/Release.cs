using System;
using System.ComponentModel.DataAnnotations;

namespace RDMUI.Models
{
    public class Release : ChangeControlElement
    {
        public string SystemID {get; set;}
        public bool Active {get;set;}
        public string Name {get; set;}
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeploymentDate {get;set;}
    }
}