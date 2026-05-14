using CNPM_LIBRARY_MANAGEMENT.Data.Models;

namespace CNPM_LIBRARY_MANAGEMENT.Models
{
    public class MegaMenuViewModel
    {
        public IEnumerable<TheLoaiSach> TheLoaiSaches { get; set; }
        public IEnumerable<LoaiVppham> LoaiVpphams { get; set; }
    }
}