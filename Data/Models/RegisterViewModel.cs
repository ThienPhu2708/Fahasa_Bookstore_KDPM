using System.ComponentModel.DataAnnotations;

namespace CNPM_LIBRARY_MANAGEMENT.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Tên hiển thị")]
        [Display(Name = "Tên hiển thị")]
        public string? TenNguoiDung { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Gmail")]
        [EmailAddress(ErrorMessage = "Định dạng Gmail không hợp lệ")]
        public string Gmail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}