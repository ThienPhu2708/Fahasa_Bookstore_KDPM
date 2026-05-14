using System.ComponentModel.DataAnnotations;

namespace CNPM_LIBRARY_MANAGEMENT.Models 
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
    }
}