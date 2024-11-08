using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ObjectModel
    {
        public int OrderIndex { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public dynamic Value { get; set; }
    }
}