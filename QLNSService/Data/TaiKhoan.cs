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
    
    public partial class TaiKhoan
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int ThanhPham { get; set; }
        public int BTP { get; set; }
        public Nullable<int> ThaoTac { get; set; }
        public Nullable<int> Floor { get; set; }
        public string ListChuyenId { get; set; }
        public bool IsOnwer { get; set; }
        public bool IsKanbanAcc { get; set; }
        public bool IsCompleteAcc { get; set; }
    }
}
