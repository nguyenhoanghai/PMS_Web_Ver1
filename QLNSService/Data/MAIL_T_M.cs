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
    
    public partial class MAIL_T_M
    {
        public int Id { get; set; }
        public int MailTemplateId { get; set; }
        public int MailFileId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual MAIL_FILE MAIL_FILE { get; set; }
        public virtual MAIL_TEMPLATE MAIL_TEMPLATE { get; set; }
    }
}
