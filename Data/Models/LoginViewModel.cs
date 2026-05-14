using System.ComponentModel.DataAnnotations;

namespace CNPM_LIBRARY_MANAGEMENT.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Gmail")]
        [EmailAddress(ErrorMessage = "Định dạng Gmail không hợp lệ")]
        public string Gmail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // [Display(Name = "Ghi nhớ tôi?")]
        // public bool RememberMe { get; set; }
    }
}