namespace CafeManagement.Web.Models;

// Nhân viên (dùng cho đăng nhập và phân quyền ở app quản trị)
public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Staff"; // Admin / Staff
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
}
