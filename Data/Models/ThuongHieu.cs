using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class ThuongHieu
{
    public int Id { get; set; }

    public string? MaThuongHieu { get; set; }

    public string TenThuongHieu { get; set; } = null!;

    public virtual ICollection<ChiTietVpp> ChiTietVpps { get; set; } = new List<ChiTietVpp>();
}
