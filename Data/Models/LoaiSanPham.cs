using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class LoaiSanPham
{
    public int Id { get; set; }

    public string? IdloaiSp { get; set; }

    public string TenLoaiSp { get; set; } = null!;

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
