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
    
    public partial class HR_InsuranceDetail
    {
        public int InsuranceDetailID { get; set; }
        public int InsuranceID { get; set; }
        public Nullable<int> HospitalID { get; set; }
        public int EmployeeID { get; set; }
        public string NumberInsurance { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
        public Nullable<System.DateTime> DateOfIssue { get; set; }
        public Nullable<System.DateTime> ValidFrom { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
        public string CareCode { get; set; }
        public string ProvinceCode { get; set; }
        public string Note { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> NumberChange { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> InsuranceType { get; set; }
    
        public virtual HR_Employee HR_Employee { get; set; }
        public virtual HR_InsuranceConfig HR_InsuranceConfig { get; set; }
    }
}
