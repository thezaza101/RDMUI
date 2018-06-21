using System;

namespace RDMUI.Models
{
    public class Release : ChangeControlElement
    {
        public string SystemID {get; set;}
        public bool Active {get;set;}
        public string Name {get; set;}
        public DateTime DeploymentDate {get;set;}
    }
}