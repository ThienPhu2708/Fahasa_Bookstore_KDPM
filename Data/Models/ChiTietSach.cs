using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class ChiTietSach
{
    public int SanPhamId { get; set; }

    public int TheLoaiId { get; set; }

    public int? TacGiaId { get; set; }

    public int? NhaXuatBanId { get; set; }

    public int? SoTrang { get; set; }

    public string? LoaiBia { get; set; }

    public DateOnly? NgayPhatHanh { get; set; }

    public string? KichThuoc { get; set; }

    public virtual NhaXuatBan? NhaXuatBan { get; set; }

    public virtual SanPham SanPham { get; set; } = null!;

    public virtual TacGium? TacGia { get; set; }

    public virtual TheLoaiSach TheLoai { get; set; } = null!;
}
