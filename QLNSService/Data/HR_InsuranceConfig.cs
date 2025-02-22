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
    
    public partial class HR_InsuranceConfig
    {
        public HR_InsuranceConfig()
        {
            this.HR_InsuranceDetail = new HashSet<HR_InsuranceDetail>();
        }
    
        public int InsuranceID { get; set; }
        public string Name { get; set; }
        public double EmployeePercent { get; set; }
        public int CompanyID { get; set; }
        public double CompanyPercent { get; set; }
        public double TotalPercent { get; set; }
        public System.DateTime ValidFrom { get; set; }
        public System.DateTime ValidTo { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> InsuranceType { get; set; }
    
        public virtual HR_InsuranceType HR_InsuranceType { get; set; }
        public virtual ICollection<HR_InsuranceDetail> HR_InsuranceDetail { get; set; }
    }
}
