using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class ChiTietVpp
{
    public int SanPhamId { get; set; }

    public int IdloaiVpp { get; set; }

    public int? ThuongHieuId { get; set; }

    public int? NhaSanXuatId { get; set; }

    public virtual LoaiVppham IdloaiVppNavigation { get; set; } = null!;

    public virtual NhaSanXuat? NhaSanXuat { get; set; }

    public virtual SanPham SanPham { get; set; } = null!;

    public virtual ThuongHieu? ThuongHieu { get; set; }

    public virtual ICollection<ChatLieu> ChatLieus { get; set; } = new List<ChatLieu>();

    public virtual ICollection<MauSac> MauSacs { get; set; } = new List<MauSac>();
}
