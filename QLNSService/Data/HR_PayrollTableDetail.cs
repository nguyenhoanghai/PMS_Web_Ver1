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
    
    public partial class HR_PayrollTableDetail
    {
        public HR_PayrollTableDetail()
        {
            this.HR_PayrollTableDetailData = new HashSet<HR_PayrollTableDetailData>();
        }
    
        public int PayrollTableDetailID { get; set; }
        public int PayrollTableID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DataType { get; set; }
        public string Formula { get; set; }
        public Nullable<int> ColumnIndex { get; set; }
        public string Description { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> DeleteUser { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public int CompanyID { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual HR_PayrollTable HR_PayrollTable { get; set; }
        public virtual ICollection<HR_PayrollTableDetailData> HR_PayrollTableDetailData { get; set; }
    }
}
