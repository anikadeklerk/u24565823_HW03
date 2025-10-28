using BikeStores_MVC.Models;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

public class ReportsController : Controller
{
    private readonly BikeStoresEntities1 db = new BikeStoresEntities1();
    private readonly string archivePath = "~/ReportArchive/";

    // GET: Reports
    public async Task<ActionResult> Index(int? customerId)
    {
        ViewBag.CustomerId = new SelectList(await db.customers.OrderBy(c => c.first_name).ToListAsync(),
                                            "customer_id", "first_name", customerId);
        OrderHistoryReportVM report = null;

        if (customerId != null)
        {
            var cust = await db.customers.FindAsync(customerId);
            report = new OrderHistoryReportVM
            {
                CustomerName = cust.first_name + " " + cust.last_name,
                Orders = await db.order_items
                                .Where(o => o.order.customer_id == customerId)
                                .Select(o => new OrderDetailItem
                                {
                                    OrderId = o.order_id,
                                    OrderDate = o.order.order_date,
                                    Product = o.product.product_name,
                                    UnitPrice = o.list_price,
                                    Quantity = o.quantity
                                }).OrderBy(o => o.OrderDate).ToListAsync()
            };
        }

        // list saved reports for archive table
        var dir = new DirectoryInfo(HttpContext.Server.MapPath(archivePath));
        if (!dir.Exists) dir.Create();
        var files = dir.GetFiles().OrderByDescending(f => f.CreationTime)
                       .Select(f => new { f.Name, f.Length, Date = f.CreationTime });
        ViewBag.Files = files;
        return View(report);
    }

    [HttpPost]
    public async Task<ActionResult> SaveReport(HttpPostedFileBase file, string description)
    {
        var dir = Server.MapPath(archivePath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        if (file != null && file.ContentLength > 0)
        {
            var path = Path.Combine(dir, Path.GetFileName(file.FileName));
            file.SaveAs(path);

            // store description
            System.IO.File.WriteAllText(path + ".desc.txt", description ?? "");
        }
        return RedirectToAction("Index");
    }

    public FileResult Download(string file)
    {
        var path = Server.MapPath(archivePath + file);
        return File(path, "application/octet-stream", file);
    }

    public ActionResult Delete(string file)
    {
        var path = Server.MapPath(archivePath + file);
        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        var desc = path + ".desc.txt";
        if (System.IO.File.Exists(desc)) System.IO.File.Delete(desc);
        return RedirectToAction("Index");
    }
}
