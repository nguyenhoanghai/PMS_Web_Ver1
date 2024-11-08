using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class CheckListModel
    {
        public string Title { get; set; }
        public List<dynamic> Head { get; set; }
        public List<BodyItem> Body { get; set; }
        public CheckListModel() {
            Head = new List<dynamic>();
            Body = new List<BodyItem>();
        }
    }
    public class BodyItem {
        public List<dynamic> Items { get; set; }
        public List<int> ItemIds { get; set; }
        public BodyItem()
        {
            Items = new List<dynamic>();
            ItemIds = new List<int>();
        }
    }
}