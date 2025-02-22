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
    
    public partial class SModule
    {
        public SModule()
        {
            this.SCheckList_Config = new HashSet<SCheckList_Config>();
            this.SFeatures = new HashSet<SFeature>();
            this.SMenus = new HashSet<SMenu>();
            this.SMenuCategories = new HashSet<SMenuCategory>();
            this.SRolePermissions = new HashSet<SRolePermission>();
        }
    
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string ModuleName { get; set; }
        public bool IsSystem { get; set; }
        public int OrderIndex { get; set; }
        public string Description { get; set; }
        public string ModuleUrl { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public bool IsShow { get; set; }
    
        public virtual ICollection<SCheckList_Config> SCheckList_Config { get; set; }
        public virtual ICollection<SFeature> SFeatures { get; set; }
        public virtual ICollection<SMenu> SMenus { get; set; }
        public virtual ICollection<SMenuCategory> SMenuCategories { get; set; }
        public virtual ICollection<SRolePermission> SRolePermissions { get; set; }
    }
}
