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
    
    public partial class P_CompletionPhase_Daily
    {
        public int Id { get; set; }
        public int AssignId { get; set; }
        public int CompletionPhaseId { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }
        public int CommandTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual P_AssignCompletion P_AssignCompletion { get; set; }
        public virtual P_CompletionPhase P_CompletionPhase { get; set; }
    }
}
