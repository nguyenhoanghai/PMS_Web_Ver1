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
    
    public partial class HR_PayrollSumary
    {
        public int PayrollSumaryID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CycleWageID { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> DeleteUser { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public int CompanyID { get; set; }
        public bool IsDeleted { get; set; }
        public decimal SumAllowances { get; set; }
        public decimal RealAllowances { get; set; }
        public bool IsLocked { get; set; }
        public bool IsApproved { get; set; }
        public Nullable<int> OrganizationID { get; set; }
    
        public virtual HR_PayrollCycleWage HR_PayrollCycleWage { get; set; }
    }
}
