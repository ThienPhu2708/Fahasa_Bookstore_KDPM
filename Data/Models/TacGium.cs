using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class TacGium
{
    public int Id { get; set; }

    public string? MaTacGia { get; set; }

    public string TenTacGia { get; set; } = null!;

    public virtual ICollection<ChiTietSach> ChiTietSaches { get; set; } = new List<ChiTietSach>();
}
