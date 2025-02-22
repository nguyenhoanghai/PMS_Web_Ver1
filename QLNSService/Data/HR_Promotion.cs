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
    
    public partial class HR_Promotion
    {
        public int PromotionId { get; set; }
        public int EmployeeId { get; set; }
        public int OldOrganizarionId { get; set; }
        public int OldPositionId { get; set; }
        public int OldBusinessId { get; set; }
        public int NewOrganizationID { get; set; }
        public int NewPositionID { get; set; }
        public int NewBusinessId { get; set; }
        public System.DateTime ValidDateFrom { get; set; }
        public System.DateTime ValidDateTo { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual HR_Employee HR_Employee { get; set; }
        public virtual HR_JobTitle HR_JobTitle { get; set; }
        public virtual HR_Organization HR_Organization { get; set; }
        public virtual HR_Position HR_Position { get; set; }
    }
}
