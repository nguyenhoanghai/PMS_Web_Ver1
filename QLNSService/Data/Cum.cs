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
    
    public partial class Cum
    {
        public Cum()
        {
            this.BTPs = new HashSet<BTP>();
            this.KeyPad_Object = new HashSet<KeyPad_Object>();
            this.NangSuat_Cum = new HashSet<NangSuat_Cum>();
            this.NangSuat_CumLoi = new HashSet<NangSuat_CumLoi>();
            this.P_Keypad = new HashSet<P_Keypad>();
            this.TheoDoiNgays = new HashSet<TheoDoiNgay>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string TenCum { get; set; }
        public string MoTa { get; set; }
        public int IdChuyen { get; set; }
        public Nullable<int> FloorId { get; set; }
        public bool IsEndOfLine { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<BTP> BTPs { get; set; }
        public virtual Chuyen Chuyen { get; set; }
        public virtual ICollection<KeyPad_Object> KeyPad_Object { get; set; }
        public virtual ICollection<NangSuat_Cum> NangSuat_Cum { get; set; }
        public virtual ICollection<NangSuat_CumLoi> NangSuat_CumLoi { get; set; }
        public virtual ICollection<P_Keypad> P_Keypad { get; set; }
        public virtual ICollection<TheoDoiNgay> TheoDoiNgays { get; set; }
    }
}
