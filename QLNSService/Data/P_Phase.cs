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
    
    public partial class P_Phase
    {
        public P_Phase()
        {
            this.P_PhaseDailyLog = new HashSet<P_PhaseDailyLog>();
            this.P_Phase_Assign_Log = new HashSet<P_Phase_Assign_Log>();
        }
    
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool IsShow { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<P_PhaseDailyLog> P_PhaseDailyLog { get; set; }
        public virtual ICollection<P_Phase_Assign_Log> P_Phase_Assign_Log { get; set; }
    }
}
