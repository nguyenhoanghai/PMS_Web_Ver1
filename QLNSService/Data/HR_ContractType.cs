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
    
    public partial class HR_ContractType
    {
        public HR_ContractType()
        {
            this.HR_ContractDetail = new HashSet<HR_ContractDetail>();
        }
    
        public int ContractTypeId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<int> DeleteUser { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<HR_ContractDetail> HR_ContractDetail { get; set; }
    }
}
