using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class NhaSanXuat
{
    public int Id { get; set; }

    public string? MaNsx { get; set; }

    public string TenNsx { get; set; } = null!;

    public virtual ICollection<ChiTietVpp> ChiTietVpps { get; set; } = new List<ChiTietVpp>();
}
