using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class ChiTietHoaDon
{
    public int Id { get; set; }

    public int HoaDonId { get; set; }

    public int SanPhamId { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual SanPham SanPham { get; set; } = null!;
}
