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
    
    public partial class HR_OverTimeObject
    {
        public HR_OverTimeObject()
        {
            this.HR_OverTimeObject_Detail = new HashSet<HR_OverTimeObject_Detail>();
            this.HR_OverTimeObject_Detail1 = new HashSet<HR_OverTimeObject_Detail>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Nullable<int> TypeOfOverTime { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public Nullable<int> BreakScheduleId { get; set; }
        public int ApprovalUserId { get; set; }
        public System.DateTime FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> TimeTypeGroupId { get; set; }
        public bool Approval { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual ICollection<HR_OverTimeObject_Detail> HR_OverTimeObject_Detail { get; set; }
        public virtual ICollection<HR_OverTimeObject_Detail> HR_OverTimeObject_Detail1 { get; set; }
    }
}
