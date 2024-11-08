using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class InsertProductivityModel{    
        public int Id { get; set; }
        public int LineId { get; set; }
        public int ClusterId { get; set; }
        public int ProductId { get; set; }
        public int TypeOfProductivity { get; set; }
        public int ErrorId { get; set; }
        public int Quantity { get; set; }
        public bool IsIncrease { get; set; }
    }
}