using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace exceldoc.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Region { get; set; }
        public string Person { get; set; }
        public string Item { get; set; }
        public decimal Units { get; set; }
       
        [DisplayName("Unit Cost")]
        public decimal UnitCost { get; set; }
        public decimal Total { get; set; }
    }
}