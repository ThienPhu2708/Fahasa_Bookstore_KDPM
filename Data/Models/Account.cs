using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class Account
{
    public int Id { get; set; }

    public string? IdnguoiDung { get; set; }

    public string? TenNguoiDung { get; set; }

    public string Gmail { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!; 
    public byte[] PasswordSalt { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? AvatarNguoiDung { get; set; }

    public string? GioiThieu { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    public virtual ICollection<GioHangItem> GioHangItems { get; set; } = new List<GioHangItem>();
}
