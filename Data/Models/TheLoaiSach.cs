using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class TheLoaiSach
{
    public int Id { get; set; }

    public string? MaTheLoai { get; set; }

    public string TenTheLoai { get; set; } = null!;

    public virtual ICollection<ChiTietSach> ChiTietSaches { get; set; } = new List<ChiTietSach>();
}
