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
    
    public partial class HR_PayrollCycleWageAllocation
    {
        public int CycleWageAllocationId { get; set; }
        public int CycleWageId { get; set; }
        public int DepartmentId { get; set; }
        public int GroupEmpId { get; set; }
        public int CompanyId { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
    
        public virtual HR_PayrollCycleWage HR_PayrollCycleWage { get; set; }
    }
}
