using System.Diagnostics;
using CafeManagement.Web.Data;
using CafeManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeManagement.Web.Controllers;

public class HomeController : Controller
{
    private readonly CafeDbContext _db;

    public HomeController(CafeDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? q, int? categoryId)
    {
        var query = _db.Products.Include(p => p.Category).Where(p => p.IsAvailable);

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.Name.Contains(q));
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Q = q;
        ViewBag.CategoryId = categoryId;

        return View(await query.OrderBy(p => p.Name).ToListAsync());
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
