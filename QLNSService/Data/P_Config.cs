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
    
    public partial class P_Config
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
