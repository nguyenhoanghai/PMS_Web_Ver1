using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ErrorReportModel
    {
        public ErrorReportModel()
        {
            SubErrors = new List<SubErrorReportModel>();
        }
        public List<SubErrorReportModel> SubErrors { get; set; }
        public string LineName { get; set; }
        public DateTime Date { get; set; }
        public float TotalErrors { get; set; }
    }

    public class SubErrorReportModel
    {
        public string  ErrorName { get; set; }
        public float Quantites { get; set; }
    }
}