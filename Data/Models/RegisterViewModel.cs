using System.ComponentModel.DataAnnotations;

namespace CNPM_LIBRARY_MANAGEMENT.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Tên hiển thị")]
        [Display(Name = "Tên hiển thị")]
        [RegularExpression(@"^(?=.*[a-zA-Z\u00C0-\u1EF9]).*$", ErrorMessage = "Tên hiển thị thiếu chữ cái")]
        public string? TenNguoiDung { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Gmail")]
        [EmailAddress(ErrorMessage = "Định dạng Gmail không hợp lệ")]
        public string Gmail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=\[{\]};:<>|./?,-]).*$", ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ in hoa, 1 chữ số, và 1 ký tự đặc biệt")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}