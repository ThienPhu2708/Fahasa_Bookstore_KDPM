using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class MauSac
{
    public int Id { get; set; }

    public string? MaMauSac { get; set; }

    public string TenMauSac { get; set; } = null!;

    public virtual ICollection<ChiTietVpp> SanPhamVpps { get; set; } = new List<ChiTietVpp>();
}
