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
    
    public partial class P_KeypadRegis_Detail
    {
        public int Id { get; set; }
        public int KeypadRegisId { get; set; }
        public int PhaseId { get; set; }
        public int Index { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual P_DailyKeypadRegister P_DailyKeypadRegister { get; set; }
    }
}
