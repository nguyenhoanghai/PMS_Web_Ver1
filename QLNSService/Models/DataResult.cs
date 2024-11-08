using QLNSService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class DataResult
    {
        public string Result { get; set; }   
        public dynamic Data { get; set; }        
        public List<string> ErrorMessages { get; set; }        
        public string Message { get; set; }
        public DataResult()
        {
            ErrorMessages = new List<string>();            
        }
    }
}