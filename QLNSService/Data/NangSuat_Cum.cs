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
    
    public partial class NangSuat_Cum
    {
        public int Id { get; set; }
        public string Ngay { get; set; }
        public int STTChuyen_SanPham { get; set; }
        public int IdCum { get; set; }
        public int SanLuongKCSTang { get; set; }
        public int SanLuongKCSGiam { get; set; }
        public int SanLuongTCTang { get; set; }
        public int SanLuongTCGiam { get; set; }
        public int BTPTang { get; set; }
        public int BTPGiam { get; set; }
        public int BTP_HC_Tang { get; set; }
        public int BTP_HC_Giam { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual Cum Cum { get; set; }
    }
}
