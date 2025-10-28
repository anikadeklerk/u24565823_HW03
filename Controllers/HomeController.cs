using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BikeStores_MVC.Models;

public class HomeController : Controller
{
    private readonly BikeStoresEntities1 db = new BikeStoresEntities1();

    // GET: /
    public async Task<ActionResult> Index(int? brandId, int? categoryId,
                                         int staffPage = 1, int custPage = 1, int prodPage = 1,
                                         int pageSize = 10)
    {
        // Build product query with related lookups (no FK ids shown)
        var productsQ = db.products
            .Include(p => p.brand)
            .Include(p => p.category)
            .AsQueryable();

        if (brandId.HasValue) productsQ = productsQ.Where(p => p.brand_id == brandId.Value);
        if (categoryId.HasValue) productsQ = productsQ.Where(p => p.category_id == categoryId.Value);

        // Independent "paging" (no AJAX, simple Skip/Take per section)
        var staffQ = db.staffs.OrderBy(s => s.first_name);
        var customersQ = db.customers.OrderBy(c => c.first_name);

        var vm = new HomePageVM
        {
            BrandId = brandId,
            CategoryId = categoryId,
            BrandList = new SelectList(await db.brands.OrderBy(b => b.brand_name).ToListAsync(), "brand_id", "brand_name", brandId),
            CategoryList = new SelectList(await db.categories.OrderBy(c => c.category_name).ToListAsync(), "category_id", "category_name", categoryId),

            Staff = await staffQ.Skip((staffPage - 1) * pageSize).Take(pageSize).ToListAsync(),
            Customers = await customersQ.Skip((custPage - 1) * pageSize).Take(pageSize).ToListAsync(),
            Products = await productsQ.OrderBy(p => p.product_name)
                                      .Skip((prodPage - 1) * pageSize).Take(pageSize).ToListAsync(),

            StaffPage = staffPage,
            CustPage = custPage,
            ProdPage = prodPage,
            PageSize = pageSize
        };

        return View(vm);
    }

    // Optional success toast after modal create
    public ActionResult Success(string msg)
    {
        TempData["Success"] = msg;
        return RedirectToAction("Index");
    }
}
