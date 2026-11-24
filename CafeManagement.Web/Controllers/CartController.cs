using CafeManagement.Data.Models;
using CafeManagement.Data;
using CafeManagement.Web.Data;
using CafeManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeManagement.Web.Controllers;

public class CartController : Controller
{
    private const string CartKey = "cart";
    private readonly CafeDbContext _db;

    public CartController(CafeDbContext db) => _db = db;

    private List<CartItem> GetCart() => HttpContext.Session.GetJson<List<CartItem>>(CartKey) ?? new();
    private void SaveCart(List<CartItem> cart) => HttpContext.Session.SetJson(CartKey, cart);

    public IActionResult Index() => View(GetCart());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null || !product.IsAvailable) return NotFound();
        if (quantity < 1) quantity = 1;

        var cart = GetCart();
        var item = cart.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            cart.Add(new CartItem { ProductId = product.Id, Name = product.Name, Price = product.Price, Quantity = quantity });
        else
            item.Quantity += quantity;

        SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Update(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            if (quantity < 1) cart.Remove(item);
            else item.Quantity = Math.Min(quantity, 50);
            SaveCart(cart);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        var cart = GetCart();
        cart.RemoveAll(i => i.ProductId == productId);
        SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Checkout()
    {
        var cart = GetCart();
        if (!cart.Any()) return RedirectToAction(nameof(Index));
        ViewBag.Tables = await _db.CafeTables.OrderBy(t => t.Id).ToListAsync();
        return View(cart);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(string? customerName, string? customerPhone, int? tableId)
    {
        var cart = GetCart();
        if (!cart.Any()) return RedirectToAction(nameof(Index));

        if (string.IsNullOrWhiteSpace(customerName))
        {
            ModelState.AddModelError("", "Vui lòng nhập họ tên.");
            ViewBag.Tables = await _db.CafeTables.OrderBy(t => t.Id).ToListAsync();
            return View(cart);
        }

        // Lấy giá hiện tại từ database, không tin giá trong Session
        var ids = cart.Select(c => c.ProductId).ToList();
        var products = await _db.Products
            .Where(p => ids.Contains(p.Id) && p.IsAvailable)
            .ToDictionaryAsync(p => p.Id);

        var order = new Order
        {
            CustomerName = customerName.Trim(),
            CustomerPhone = customerPhone?.Trim(),
            CafeTableId = tableId,
            Status = "Mới"
        };

        foreach (var item in cart)
        {
            if (!products.TryGetValue(item.ProductId, out var p)) continue;
            order.Details.Add(new OrderDetail { ProductId = p.Id, Quantity = item.Quantity, UnitPrice = p.Price });
        }

        if (!order.Details.Any())
        {
            SaveCart(new List<CartItem>());
            return RedirectToAction(nameof(Index));
        }

        order.TotalAmount = order.Details.Sum(d => d.UnitPrice * d.Quantity);
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        SaveCart(new List<CartItem>());
        return RedirectToAction(nameof(Success), new { id = order.Id });
    }

    public async Task<IActionResult> Success(int id)
    {
        var order = await _db.Orders
            .Include(o => o.CafeTable)
            .Include(o => o.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();
        return View(order);
    }
}
