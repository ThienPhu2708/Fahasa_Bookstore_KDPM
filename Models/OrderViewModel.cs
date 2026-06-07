using System.ComponentModel.DataAnnotations;

namespace CNPM_LIBRARY_MANAGEMENT.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ tên người nhận")]
        public string HoTenNguoiNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ giao hàng")]
        [MinLength(5, ErrorMessage = "Địa chỉ quá ngắn hoặc không hợp lệ")]
        public string DiaChiGiaoHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ (phải đủ 10 chữ số)")]
        public string SoDienThoai { get; set; }
    }
}