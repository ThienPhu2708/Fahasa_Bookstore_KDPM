using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class ChatLieu
{
    public int Id { get; set; }

    public string? MaChatLieu { get; set; }

    public string TenChatLieu { get; set; } = null!;

    public virtual ICollection<ChiTietVpp> SanPhamVpps { get; set; } = new List<ChiTietVpp>();
}
