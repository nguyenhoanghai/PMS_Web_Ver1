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
    
    public partial class HR_PayrollCycleWage
    {
        public HR_PayrollCycleWage()
        {
            this.HR_ContractDetail = new HashSet<HR_ContractDetail>();
            this.HR_CycleWageHistory = new HashSet<HR_CycleWageHistory>();
            this.HR_PayrollCycleWageAllocation = new HashSet<HR_PayrollCycleWageAllocation>();
            this.HR_PayrollCycleWageDetail = new HashSet<HR_PayrollCycleWageDetail>();
            this.HR_PayrollSumary = new HashSet<HR_PayrollSumary>();
            this.HR_PayrollTableDetailData = new HashSet<HR_PayrollTableDetailData>();
            this.HR_PayrollTableInputData = new HashSet<HR_PayrollTableInputData>();
        }
    
        public int CycleWageID { get; set; }
        public string CycleWageName { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ListDepartmentID { get; set; }
    
        public virtual ICollection<HR_ContractDetail> HR_ContractDetail { get; set; }
        public virtual ICollection<HR_CycleWageHistory> HR_CycleWageHistory { get; set; }
        public virtual ICollection<HR_PayrollCycleWageAllocation> HR_PayrollCycleWageAllocation { get; set; }
        public virtual ICollection<HR_PayrollCycleWageDetail> HR_PayrollCycleWageDetail { get; set; }
        public virtual ICollection<HR_PayrollSumary> HR_PayrollSumary { get; set; }
        public virtual ICollection<HR_PayrollTableDetailData> HR_PayrollTableDetailData { get; set; }
        public virtual ICollection<HR_PayrollTableInputData> HR_PayrollTableInputData { get; set; }
    }
}
