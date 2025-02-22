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
    
    public partial class P_ProManagement
    {
        public P_ProManagement()
        {
            this.P_PM_JobGroup = new HashSet<P_PM_JobGroup>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int ObjectType { get; set; }
        public Nullable<int> ObjectId { get; set; }
        public int ParentId { get; set; }
        public string Node { get; set; }
        public Nullable<int> Owner { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual ICollection<P_PM_JobGroup> P_PM_JobGroup { get; set; }
    }
}
