using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class HoaDon
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public DateTime NgayDatHang { get; set; }

    public decimal TongTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public string? HoTenNguoiNhan { get; set; }

    public string? DiaChiGiaoHang { get; set; }

    public string? SoDienThoai { get; set; }

    public decimal GiamGia { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
}
