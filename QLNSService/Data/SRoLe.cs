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
    
    public partial class SRoLe
    {
        public SRoLe()
        {
            this.SRolePermissions = new HashSet<SRolePermission>();
            this.SUserRoles = new HashSet<SUserRole>();
        }
    
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual SCompany SCompany { get; set; }
        public virtual ICollection<SRolePermission> SRolePermissions { get; set; }
        public virtual ICollection<SUserRole> SUserRoles { get; set; }
    }
}
