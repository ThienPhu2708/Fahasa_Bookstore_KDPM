using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CNPM_LIBRARY_MANAGEMENT.Models
{
    public class ProductAdminViewModel
    {
        // --- Phần 1: Dữ liệu của SanPham (Bảng cha) ---
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [Display(Name = "Tên Sản Phẩm")]
        public string TenSanPham { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Display(Name = "Giá Bán")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal GiaBan { get; set; }

        [Display(Name = "Mô Tả")]
        public string? MoTa { get; set; }

        [Display(Name = "Hình Ảnh")]
        public string? HinhAnh { get; set; }

        [Display(Name = "Tải lên Hình ảnh")]
        public IFormFile? ImageFile { get; set; }

        [Required]
        [Display(Name = "Loại Sản Phẩm")]
        public int LoaiSanPhamId { get; set; } // 1=VPP, 2=Sách

        [Display(Name = "Giảm giá (%)")]
        [Range(0, 100, ErrorMessage = "Phần trăm giảm phải từ 0 đến 100")]
        public decimal? PhanTramGiam { get; set; }

        [Display(Name = "Hàng mới về")]
        public bool HangMoiVe { get; set; }

        // --- Phần 2: Dữ liệu của ChiTietSach ---
        [Display(Name = "Tác Giả")]
        public int? TacGiaId { get; set; }

        // === SỬA LỖI Ở ĐÂY ===
        [Display(Name = "Thể Loại Sách")]
        public int? TheLoaiId { get; set; } 

        [Display(Name = "Nhà Xuất Bản")]
        public int? NhaXuatBanId { get; set; }

        [Display(Name = "Số Trang")]
        public int? SoTrang { get; set; }

        [Display(Name = "Loại Bìa")]
        public string? LoaiBia { get; set; }

        // --- Phần 3: Dữ liệu của ChiTietVpp ---
        [Display(Name = "Loại VPP")]
        public int? IdLoaiVpp { get; set; }

        [Display(Name = "Thương Hiệu")]
        public int? ThuongHieuId { get; set; }

        [Display(Name = "Nhà Sản Xuất (VPP)")]
        public int? NhaSanXuatId { get; set; }

        // --- Phần 4: Dữ liệu cho Dropdown Lists ---
        public IEnumerable<SelectListItem>? LoaiSanPhams { get; set; }
        public IEnumerable<SelectListItem>? TacGias { get; set; }

        public IEnumerable<SelectListItem>? TheLoaiSaches { get; set; } 

        public IEnumerable<SelectListItem>? NhaXuatBans { get; set; }
        public IEnumerable<SelectListItem>? LoaiVpphams { get; set; }
        public IEnumerable<SelectListItem>? ThuongHieus { get; set; }
        public IEnumerable<SelectListItem>? NhaSanXuats { get; set; }

        public IEnumerable<SelectListItem>? AllMauSacs { get; set; }
        public List<int>? SelectedMauSacIds { get; set; }

        public IEnumerable<SelectListItem>? AllChatLieus { get; set; }
        public List<int>? SelectedChatLieuIds { get; set; }
    }
}