using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ProductivitiesOfLineByHoursModel
    {
        public ProductivitiesOfLineByHoursModel()
        {
            Productivities = new List<ProductivitiesByHoursModel>();
        }
        public string LineName { get; set; }
        public DateTime Date { get; set; } 
        public List<ProductivitiesByHoursModel> Productivities { get; set; }
        public float ProductivitiesOfLine { get; set; }  //năng suất ngày
        public double NormsDay { get; set; }  // định mức ngày
    }

    public class ProductivitiesByHoursModel
    { 
        public string HourName { get; set; }
        public float Value { get; set; }    // năng suất giờ
        public double NormsHour { get; set; }   // định mức giờ
    }
}