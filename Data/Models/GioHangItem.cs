using System;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public class GioHangItem
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int SanPhamId { get; set; }
    public decimal DonGia { get; set; }
    public int SoLuong { get; set; }

    public virtual Account Account { get; set; } = null!;
    public virtual SanPham SanPham { get; set; } = null!;
}
