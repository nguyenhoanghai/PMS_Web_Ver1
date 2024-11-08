using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class BTP_HCStructureModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsShow { get; set; }
    }
}