using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class LoginUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LineIds_Str { get; set; }
        public bool IsKanbanAccount { get; set; }
        public bool IsAccountUser { get; set; } 
    }
}