using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using CNPM_LIBRARY_MANAGEMENT.Models;
using CNPM_LIBRARY_MANAGEMENT.Helpers; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNPM_LIBRARY_MANAGEMENT.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private const string CartSessionKey = "Cart";

        // GET: /Cart/Index 
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart); 
            // Trả về Views/Cart/Index.cshtml
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int sanPhamId, int quantity = 1)
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            var cartItem = cart.FirstOrDefault(x => x.SanPhamId == sanPhamId);

            if (cartItem != null)
            {
                cartItem.SoLuong += quantity;
            }
            else
            {
                var sanPham = _context.SanPhams.Find(sanPhamId);
                if (sanPham != null)
                {
                    var donGia = (sanPham.PhanTramGiam.HasValue && sanPham.PhanTramGiam > 0)
                        ? Math.Round(sanPham.GiaBan * (1 - sanPham.PhanTramGiam.Value / 100m), 0)
                        : sanPham.GiaBan;

                    cart.Add(new CartItemViewModel
                    {
                        SanPhamId = sanPham.Id,
                        TenSanPham = sanPham.TenSanPham,
                        DonGia = donGia,
                        SoLuong = quantity,
                        HinhAnh = sanPham.HinhAnh
                    });
                }
            }
            HttpContext.Session.Set("Cart", cart);
            await SyncCartToDbAsync(cart);

            TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng thành công!";

            string referer = Request.Headers["Referer"].ToString();
            return !string.IsNullOrEmpty(referer) ? Redirect(referer) : RedirectToAction("Index", "Home");
        }

        // POST: /Cart/RemoveFromCart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int sanPhamId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(ci => ci.SanPhamId == sanPhamId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                await SyncCartToDbAsync(cart);
            }

            return RedirectToAction("Index");
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int sanPhamId, int soLuong)
        {
            if (soLuong <= 0)
            {
                return await RemoveFromCart(sanPhamId);
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(ci => ci.SanPhamId == sanPhamId);

            if (item != null)
            {
                item.SoLuong = soLuong;
                SaveCart(cart);
                await SyncCartToDbAsync(cart);
            }

            return RedirectToAction("Index");
        }



        // Wrapper public để AccountController gọi được
        public List<CartItemViewModel> GetCartPublic() => GetCart();
        public async Task SyncCartToDbPublicAsync(List<CartItemViewModel> cart) => await SyncCartToDbAsync(cart);

        // Đồng bộ giỏ hàng session → DB nếu user đang đăng nhập
        private async Task SyncCartToDbAsync(List<CartItemViewModel> cart)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int accountId)) return;

            var existing = _context.GioHangItems.Where(g => g.AccountId == accountId).ToList();
            _context.GioHangItems.RemoveRange(existing);

            foreach (var item in cart)
            {
                _context.GioHangItems.Add(new Data.Models.GioHangItem
                {
                    AccountId = accountId,
                    SanPhamId = item.SanPhamId,
                    DonGia = item.DonGia,
                    SoLuong = item.SoLuong
                });
            }
            await _context.SaveChangesAsync();
        }

        // Nạp giỏ hàng từ DB về session (gọi sau khi login)
        public async Task LoadCartFromDbAsync(int accountId)
        {
            var dbItems = await _context.GioHangItems
                .Where(g => g.AccountId == accountId)
                .Include(g => g.SanPham)
                .ToListAsync();

            var sessionCart = GetCart();

            foreach (var dbItem in dbItems)
            {
                var existing = sessionCart.FirstOrDefault(x => x.SanPhamId == dbItem.SanPhamId);
                if (existing != null)
                {
                    existing.SoLuong += dbItem.SoLuong;
                }
                else
                {
                    sessionCart.Add(new CartItemViewModel
                    {
                        SanPhamId = dbItem.SanPhamId,
                        TenSanPham = dbItem.SanPham?.TenSanPham ?? "",
                        DonGia = dbItem.DonGia,
                        SoLuong = dbItem.SoLuong,
                        HinhAnh = dbItem.SanPham?.HinhAnh
                    });
                }
            }

            SaveCart(sessionCart);
            // Cập nhật lại DB với giỏ hàng đã merge
            await SyncCartToDbAsync(sessionCart);
        }

        // === API: THÊM VÀO GIỎ HÀNG (dùng cho Postman / kiểm thử API) ===
        // POST /Cart/ApiAddToCart
        // Body (JSON): { "sanPhamId": 1, "quantity": 2 }
        [HttpPost]
        public async Task<IActionResult> ApiAddToCart([FromBody] AddToCartRequest request)
        {
            if (request == null || request.SanPhamId <= 0)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });

            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            var cartItem = cart.FirstOrDefault(x => x.SanPhamId == request.SanPhamId);

            if (cartItem != null)
            {
                cartItem.SoLuong += request.Quantity;
            }
            else
            {
                var sanPham = _context.SanPhams.Find(request.SanPhamId);
                if (sanPham == null)
                    return Ok(new { success = false, message = "Sản phẩm không tồn tại." });

                var donGia = (sanPham.PhanTramGiam.HasValue && sanPham.PhanTramGiam > 0)
                    ? Math.Round(sanPham.GiaBan * (1 - sanPham.PhanTramGiam.Value / 100m), 0)
                    : sanPham.GiaBan;

                cart.Add(new CartItemViewModel
                {
                    SanPhamId = sanPham.Id,
                    TenSanPham = sanPham.TenSanPham,
                    DonGia = donGia,
                    SoLuong = request.Quantity,
                    HinhAnh = sanPham.HinhAnh
                });
            }

            HttpContext.Session.Set("Cart", cart);
            await SyncCartToDbAsync(cart);

            int cartCount = cart.Sum(c => c.SoLuong);
            return Ok(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng.", cartCount });
        }

        private List<CartItemViewModel> GetCart()
        {
            var sessionData = _httpContextAccessor.HttpContext?.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(sessionData))
            {
                return new List<CartItemViewModel>();
            }
            try
            {
                return JsonSerializer.Deserialize<List<CartItemViewModel>>(sessionData) ?? new List<CartItemViewModel>();
            }
            catch
            {
                return new List<CartItemViewModel>();
            }
        }

        private void SaveCart(List<CartItemViewModel> cart)
        {
            var sessionData = JsonSerializer.Serialize(cart);
            _httpContextAccessor.HttpContext?.Session.SetString(CartSessionKey, sessionData);

            // Cập nhật badge đếm
            _httpContextAccessor.HttpContext?.Session.SetInt32("CartCount", cart.Sum(c => c.SoLuong));
        }
    }
}