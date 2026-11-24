using CafeManagement.Data;
using CafeManagement.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeManagement.Web.Controllers;

public class ProductsController : Controller
{
    private readonly CafeDbContext _db;

    public ProductsController(CafeDbContext db) => _db = db;

    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products.Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsAvailable);
        if (product == null) return NotFound();
        return View(product);
    }
}
