using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models
{
    [Table("YeuThich")] 
    public class YeuThich
    {
        [Key]
        public int Id { get; set; }

        public int AccountId { get; set; }
        public int SanPhamId { get; set; }
        public DateTime NgayThem { get; set; }

        // Khai báo mối quan hệ (Navigation Properties)
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        [ForeignKey("SanPhamId")]
        public virtual SanPham SanPham { get; set; }
    }
}