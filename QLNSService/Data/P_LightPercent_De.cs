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
    
    public partial class P_LightPercent_De
    {
        public int Id { get; set; }
        public int LightPercentId { get; set; }
        public string ColorName { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual P_LightPercent P_LightPercent { get; set; }
    }
}
