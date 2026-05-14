using System.ComponentModel.DataAnnotations;

namespace CNPM_LIBRARY_MANAGEMENT.Models
{
    public class EditProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Gmail (Không thể thay đổi)")]
        public string Gmail { get; set; } // Thường không cho sửa Gmail

        [Required(ErrorMessage = "Vui lòng nhập Tên hiển thị")]
        [Display(Name = "Tên hiển thị")]
        public string? TenNguoiDung { get; set; }

        [Display(Name = "Giới thiệu")]
        [DataType(DataType.MultilineText)]
        public string? GioiThieu { get; set; }

        [Display(Name = "Ảnh đại diện mới")]
        public IFormFile? AvatarFile { get; set; } 

        public string? CurrentAvatar { get; set; } 
    }
}