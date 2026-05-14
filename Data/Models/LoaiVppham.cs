using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class LoaiVppham
{
    public int Id { get; set; }

    public string? IdloaiVpp { get; set; }

    public string TenLoaiVpp { get; set; } = null!;

    public virtual ICollection<ChiTietVpp> ChiTietVpps { get; set; } = new List<ChiTietVpp>();
}
