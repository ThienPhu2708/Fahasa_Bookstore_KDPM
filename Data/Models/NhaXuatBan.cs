using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class NhaXuatBan
{
    public int Id { get; set; }

    public string? MaNxb { get; set; }

    public string TenNxb { get; set; } = null!;

    public virtual ICollection<ChiTietSach> ChiTietSaches { get; set; } = new List<ChiTietSach>();
}
