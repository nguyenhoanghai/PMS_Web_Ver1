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
    
    public partial class HR_VacationDetail
    {
        public int VacationId { get; set; }
        public System.DateTime Date { get; set; }
        public byte VacationTimeId { get; set; }
        public int CompanyId { get; set; }
    
        public virtual HR_Vacation HR_Vacation { get; set; }
    }
}
