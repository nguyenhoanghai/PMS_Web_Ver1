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
    
    public partial class HR_CycleWageHistory
    {
        public int ID { get; set; }
        public int CycleWageId { get; set; }
        public int ContractDetailID { get; set; }
        public Nullable<int> CycleWageIdNew { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatesDate { get; set; }
        public Nullable<int> CycleWageIdOld { get; set; }
    
        public virtual HR_ContractDetail HR_ContractDetail { get; set; }
        public virtual HR_PayrollCycleWage HR_PayrollCycleWage { get; set; }
    }
}
