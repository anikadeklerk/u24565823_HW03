using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BikeStores_MVC.Models;

public class MaintainController : Controller
{
    private readonly BikeStoresEntities1 db = new BikeStoresEntities1();

    // GET: Maintain
    public async Task<ActionResult> Index()
    {
        var staff = await db.staffs.OrderBy(s => s.first_name).ToListAsync();
        var customers = await db.customers.OrderBy(c => c.first_name).ToListAsync();
        var products = await db.products
            .Include(p => p.brand)
            .Include(p => p.category)
            .OrderBy(p => p.product_name)
            .ToListAsync();

        var vm = new MaintainVM
        {
            Staff = staff,
            Customers = customers,
            Products = products,
            BrandList = new SelectList(await db.brands.ToListAsync(), "brand_id", "brand_name"),
            CategoryList = new SelectList(await db.categories.ToListAsync(), "category_id", "category_name")
        };
        return View(vm);
    }
}
