//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLNSService.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class HR_Hospital
    {
        public int HospitalID { get; set; }
        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
