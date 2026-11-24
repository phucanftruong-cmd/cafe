namespace CafeManagement.Data.Models;

// Bàn trong quán
public class CafeTable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Seats { get; set; }
    public string Status { get; set; } = "Trống"; // Trống / Đang dùng
}
