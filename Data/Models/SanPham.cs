using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class SanPham
{
    public int Id { get; set; }

    public string? MaSp { get; set; }

    public string TenSanPham { get; set; } = null!;

    public decimal GiaBan { get; set; }

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public decimal? TrongLuong { get; set; }

    public byte? DanhGia { get; set; }

    public int? SoLuongDaBan { get; set; }

    public DateOnly? NgayDang { get; set; }

    public int LoaiSanPhamId { get; set; }

    public decimal? PhanTramGiam { get; set; }

    public bool HangMoiVe { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ChiTietSach? ChiTietSach { get; set; }

    public virtual ChiTietVpp? ChiTietVpp { get; set; }

    public virtual LoaiSanPham LoaiSanPham { get; set; } = null!;

    public virtual ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();

    public virtual ICollection<YeuThich> YeuThichs { get; set; } = new List<YeuThich>();
}