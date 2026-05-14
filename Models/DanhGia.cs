using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 

using CNPM_LIBRARY_MANAGEMENT.Data.Models;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models 
{
    public partial class DanhGia
    {
        public int Id { get; set; }

        public int SanPhamId { get; set; }

        public int AccountId { get; set; }

        public byte Sao { get; set; }

        public string? NoiDung { get; set; }

        public DateTime NgayTao { get; set; }

        public virtual Account Account { get; set; } = null!;

        public virtual SanPham SanPham { get; set; } = null!;
    }
}