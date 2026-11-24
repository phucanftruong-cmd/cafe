using CafeManagement.Web.Models;

namespace CafeManagement.Web.Data;

public static class DbSeeder
{
    public static void Seed(CafeDbContext db)
    {
        if (db.Categories.Any()) return;

        var coffee = new Category { Name = "Cà phê", Description = "Cà phê pha máy và pha phin" };
        var tea = new Category { Name = "Trà", Description = "Trà trái cây và trà sữa" };
        var smoothie = new Category { Name = "Sinh tố", Description = "Sinh tố và nước ép tươi" };
        var cake = new Category { Name = "Bánh ngọt", Description = "Bánh dùng kèm đồ uống" };

        db.Products.AddRange(
            new Product { Name = "Cà phê đen", Price = 25000, Description = "Đậm đà, không đường", Category = coffee },
            new Product { Name = "Cà phê sữa", Price = 29000, Description = "Pha phin với sữa đặc", Category = coffee },
            new Product { Name = "Bạc xỉu", Price = 32000, Description = "Nhiều sữa, ít cà phê", Category = coffee },
            new Product { Name = "Cappuccino", Price = 45000, Description = "Espresso với bọt sữa", Category = coffee },
            new Product { Name = "Trà đào cam sả", Price = 42000, Description = "Mát lạnh, thơm mùi sả", Category = tea },
            new Product { Name = "Trà sữa trân châu", Price = 38000, Description = "Trà sữa với trân châu đen", Category = tea },
            new Product { Name = "Sinh tố bơ", Price = 45000, Description = "Bơ sáp xay mịn", Category = smoothie },
            new Product { Name = "Sinh tố xoài", Price = 40000, Description = "Xoài chín xay với sữa", Category = smoothie },
            new Product { Name = "Bánh tiramisu", Price = 48000, Description = "Vị cà phê và kem phô mai", Category = cake },
            new Product { Name = "Bánh croissant", Price = 35000, Description = "Nướng giòn, thơm bơ", Category = cake }
        );

        db.CafeTables.AddRange(Enumerable.Range(1, 10)
            .Select(i => new CafeTable { Name = $"Bàn {i}", Seats = i <= 6 ? 4 : 6 }));

        db.SaveChanges();
    }
}
