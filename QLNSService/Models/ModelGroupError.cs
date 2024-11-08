using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ModelGroupError
    {
        public int CountErrorLastHours { get; set; }
        public int CountErrorCurrentHours { get; set; }
        public int TotalError { get; set; }
        public int Id { get; set; }
        public string GroupErrorName { get; set; }
        public string LineName { get; set; }
        public List<ModelError> ListError { get; set; }
    }
}