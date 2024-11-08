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
    
    public partial class P_Keypad
    {
        public P_Keypad()
        {
            this.P_DailyInfo = new HashSet<P_DailyInfo>();
            this.P_DailyKeypadPosition = new HashSet<P_DailyKeypadPosition>();
            this.P_DailyKeypadRegister = new HashSet<P_DailyKeypadRegister>();
            this.P_DailyMapper = new HashSet<P_DailyMapper>();
        }
    
        public int Id { get; set; }
        public int ClusterId { get; set; }
        public int EquipCode { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Cum Cum { get; set; }
        public virtual ICollection<P_DailyInfo> P_DailyInfo { get; set; }
        public virtual ICollection<P_DailyKeypadPosition> P_DailyKeypadPosition { get; set; }
        public virtual ICollection<P_DailyKeypadRegister> P_DailyKeypadRegister { get; set; }
        public virtual ICollection<P_DailyMapper> P_DailyMapper { get; set; }
    }
}
