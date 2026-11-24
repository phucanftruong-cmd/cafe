namespace CafeManagement.Web.Models;

// Đơn hàng
public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Mới"; // Mới / Đang pha chế / Hoàn thành / Đã hủy
    public decimal TotalAmount { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }

    public int? CafeTableId { get; set; }
    public CafeTable? CafeTable { get; set; }

    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();
}
