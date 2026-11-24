using CafeManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManagement.Web.Data;

public class CafeDbContext : DbContext
{
    public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<CafeTable> CafeTables => Set<CafeTable>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<OrderDetail>().Property(d => d.UnitPrice).HasPrecision(18, 2);

        // Tên đăng nhập không được trùng
        modelBuilder.Entity<Employee>().HasIndex(e => e.Username).IsUnique();

        // Không cho xóa danh mục / sản phẩm nếu đã có dữ liệu liên quan
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category).WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<OrderDetail>()
            .HasOne(d => d.Product).WithMany()
            .HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Restrict);
    }
}
