using System.Collections.Generic;
using System.Web.Mvc;

namespace BikeStores_MVC.Models
{
    public class MaintainVM
    {
        public IEnumerable<staff> Staff { get; set; }
        public IEnumerable<customer> Customers { get; set; }
        public IEnumerable<product> Products { get; set; }

        // Optional dropdown filters if you want them here too
        public SelectList BrandList { get; set; }
        public SelectList CategoryList { get; set; }
    }
}
