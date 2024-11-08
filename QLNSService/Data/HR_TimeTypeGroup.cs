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
    
    public partial class HR_TimeTypeGroup
    {
        public HR_TimeTypeGroup()
        {
            this.HR_OverTime = new HashSet<HR_OverTime>();
            this.HR_SALARY_Shift = new HashSet<HR_SALARY_Shift>();
            this.HR_VacationExceptionApply = new HashSet<HR_VacationExceptionApply>();
            this.HR_VacationOverTimeApply = new HashSet<HR_VacationOverTimeApply>();
            this.HR_TimeType = new HashSet<HR_TimeType>();
        }
    
        public int TimeTypeGroupId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual ICollection<HR_OverTime> HR_OverTime { get; set; }
        public virtual ICollection<HR_SALARY_Shift> HR_SALARY_Shift { get; set; }
        public virtual ICollection<HR_VacationExceptionApply> HR_VacationExceptionApply { get; set; }
        public virtual ICollection<HR_VacationOverTimeApply> HR_VacationOverTimeApply { get; set; }
        public virtual ICollection<HR_TimeType> HR_TimeType { get; set; }
    }
}
