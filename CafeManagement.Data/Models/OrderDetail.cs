namespace CafeManagement.Data.Models;

// Chi tiết đơn hàng (mỗi dòng là một món)
public class OrderDetail
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // giá tại thời điểm bán

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }
}
