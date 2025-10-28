using System.Collections.Generic;
using System.Web.Mvc;

namespace BikeStores_MVC.Models
{
    public class HomePageVM
    {
        // Lists to display (merged page)
        public IEnumerable<staff> Staff { get; set; }
        public IEnumerable<customer> Customers { get; set; }
        public IEnumerable<product> Products { get; set; }

        // Filter UI (brand + category)
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public SelectList BrandList { get; set; }
        public SelectList CategoryList { get; set; }

        // Optional: simple independent paging knobs (no AJAX)
        public int StaffPage { get; set; }
        public int CustPage { get; set; }
        public int ProdPage { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
