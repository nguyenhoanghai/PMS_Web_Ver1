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
    
    public partial class P_PlayVideoSheduleDetail
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int VideoSheduleId { get; set; }
        public int OrderIndex { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual P_PlayVideoShedule P_PlayVideoShedule { get; set; }
        public virtual P_VideoLibrary P_VideoLibrary { get; set; }
    }
}
