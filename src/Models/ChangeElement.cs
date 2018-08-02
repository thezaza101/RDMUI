using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RDMUI.Models
{
    public class ChangeControlElement : DBElement
    {
        public Dictionary<string, object> Properties = new Dictionary<string, object>();
    } 
}