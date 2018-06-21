using System;
using System.Collections.Generic;

namespace RDMUI.Models
{
    public class ChangeSet : ChangeControlElement
    {
        public string ReleaseID {get;set;}
        public bool Active {get;set;}
        public StatusCode ChangeSetStatus {get;set;}
        public string Name {get; set;}
        public List<Change> Changes = new List<Change>();
    }
}