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
    
    public partial class HR_EmployeeJob
    {
        public int EmployeeJobId { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public string Job { get; set; }
        public string JobAddress { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public bool IsApprove { get; set; }
    
        public virtual HR_Employee HR_Employee { get; set; }
    }
}
