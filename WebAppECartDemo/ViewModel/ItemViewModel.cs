using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppECartDemo.ViewModel
{
    public class ItemViewModel
    {
        public Guid ItemId { get; set; }
        public int CategoryId { get; set; }
        public string ItemCode { get; set; }

        public string ItemName { get; set; }
        public string Description { get; set; }
        public HttpPostedFile ImagePath { get; set; }
        public decimal ImagePrice { get; set; }
        public  IEnumerable<SelectListItem> CategorySelectedListItems { get; set; }
        
    }
}