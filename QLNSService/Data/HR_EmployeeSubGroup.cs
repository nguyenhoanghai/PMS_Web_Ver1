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
    
    public partial class HR_EmployeeSubGroup
    {
        public HR_EmployeeSubGroup()
        {
            this.HR_Employee = new HashSet<HR_Employee>();
            this.HR_VacationExceptionApply = new HashSet<HR_VacationExceptionApply>();
            this.HR_VacationOverTimeApply = new HashSet<HR_VacationOverTimeApply>();
        }
    
        public int EmployeeSubGroupId { get; set; }
        public string EmployeeSubGroupText { get; set; }
        public int EmployeeGroupId { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual ICollection<HR_Employee> HR_Employee { get; set; }
        public virtual HR_EmployeeGroup HR_EmployeeGroup { get; set; }
        public virtual ICollection<HR_VacationExceptionApply> HR_VacationExceptionApply { get; set; }
        public virtual ICollection<HR_VacationOverTimeApply> HR_VacationOverTimeApply { get; set; }
    }
}
