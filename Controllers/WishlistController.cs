using CNPM_LIBRARY_MANAGEMENT.Data;
using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CNPM_LIBRARY_MANAGEMENT.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishlistController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /Wishlist/Index
        public async Task<IActionResult> Index()
        {
            var userIdStr = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int uId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách sản phẩm từ bảng YeuThich
            var wishlistItems = await _context.YeuThichs
                .Where(y => y.AccountId == uId)
                .Include(y => y.SanPham) 
                .OrderByDescending(y => y.NgayThem)
                .Select(y => y.SanPham)
                .ToListAsync();

            return View(wishlistItems);
        }

        // Action này nhận AJAX request khi bấm tim
        [HttpPost]
        public async Task<IActionResult> ToggleWishlist(int sanPhamId)
        {
            // 1. Kiểm tra đăng nhập
            var userIdString = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int accountId))
            {
                // Trả về lỗi 401 nếu chưa đăng nhập
                return Unauthorized(new { message = "Vui lòng đăng nhập để lưu yêu thích." });
            }

            // 2. Kiểm tra xem đã thích chưa
            var existingItem = await _context.YeuThichs
                .FirstOrDefaultAsync(y => y.AccountId == accountId && y.SanPhamId == sanPhamId);

            if (existingItem != null)
            {
                _context.YeuThichs.Remove(existingItem);
                await _context.SaveChangesAsync();
                return Ok(new { isWishlisted = false, message = "Đã bỏ thích" });
            }
            else
            {
                var newItem = new YeuThich
                {
                    AccountId = accountId,
                    SanPhamId = sanPhamId,
                    NgayThem = DateTime.Now
                };
                _context.YeuThichs.Add(newItem);
                await _context.SaveChangesAsync();
                return Ok(new { isWishlisted = true, message = "Đã thêm vào yêu thích" });
            }
        }
    }
}