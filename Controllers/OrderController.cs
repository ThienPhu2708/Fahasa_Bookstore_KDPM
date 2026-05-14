using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using CNPM_LIBRARY_MANAGEMENT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CNPM_LIBRARY_MANAGEMENT.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private const string CartSessionKey = "Cart";

        // GET: /Order/Checkout
        [HttpGet]
        public IActionResult Checkout()
        {
            var userIdString = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var viewModel = new CheckoutViewModel
            {
                CartItems = cart
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel viewModel)
        {
            // 1. Kiểm tra đăng nhập
            var userIdString = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int accountId))
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Lấy giỏ hàng
            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            viewModel.CartItems = cart;

            if (ModelState.IsValid)
            {
                // ==============================================================
                // LOGIC TÍNH TOÁN GIẢM GIÁ (VOUCHER TỰ ĐỘNG)
                // ==============================================================

                // A. Tính tổng số lượng và tổng tiền gốc
                int totalQuantity = cart.Sum(item => item.SoLuong);
                decimal originalTotal = cart.Sum(item => item.ThanhTien);
                decimal discountRate = 0; 

                // B. Xét điều kiện giảm giá theo số lượng
                if (totalQuantity > 50)
                {
                    discountRate = 0.4m;
                }
                else if (totalQuantity > 20)
                {
                    discountRate = 0.3m; 
                }
                else if (totalQuantity > 10)
                {
                    discountRate = 0.2m; 
                }

                // C. Tính ra tiền giảm và tiền phải trả
                decimal discountAmount = originalTotal * discountRate;
                decimal finalTotal = originalTotal - discountAmount;

                // ==============================================================

                // 3. Tạo Hóa Đơn (Lưu giá đã giảm)
                var hoaDon = new HoaDon
                {
                    AccountId = accountId,
                    NgayDatHang = DateTime.Now,

                    TongTien = finalTotal,
                    GiamGia = discountAmount, 
                                              

                    TrangThai = "Chờ xử lý",
                    HoTenNguoiNhan = viewModel.OrderInfo.HoTenNguoiNhan,
                    DiaChiGiaoHang = viewModel.OrderInfo.DiaChiGiaoHang,
                    SoDienThoai = viewModel.OrderInfo.SoDienThoai
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                // 4. Lưu Chi Tiết & Cập nhật Kho
                foreach (var item in cart)
                {
                    // Lưu chi tiết hóa đơn
                    var chiTiet = new ChiTietHoaDon
                    {
                        HoaDonId = hoaDon.Id,
                        SanPhamId = item.SanPhamId,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia
                    };
                    _context.ChiTietHoaDons.Add(chiTiet);

                    // Cập nhật số lượng đã bán
                    var sanPham = await _context.SanPhams.FindAsync(item.SanPhamId);
                    if (sanPham != null)
                    {
                        sanPham.SoLuongDaBan = (sanPham.SoLuongDaBan ?? 0) + item.SoLuong;

                        // Trừ tồn kho (Logic bổ sung cho chặt chẽ)
                        // if(sanPham.SoLuongTon >= item.SoLuong) sanPham.SoLuongTon -= item.SoLuong;

                        _context.SanPhams.Update(sanPham);
                    }
                }

                await _context.SaveChangesAsync();

                // 5. Xóa giỏ hàng
                _httpContextAccessor.HttpContext?.Session.Remove(CartSessionKey);
                _httpContextAccessor.HttpContext?.Session.SetInt32("CartCount", 0);

                return RedirectToAction("OrderConfirmation", new { id = hoaDon.Id });
            }

            return View(viewModel);
        }

        // Trang xác nhận đặt hàng thành công
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var userIdString = _httpContextAccessor.HttpContext?.Session.GetString("UserId");

            var hoaDon = await _context.HoaDons
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hoaDon == null || hoaDon.AccountId.ToString() != userIdString)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(hoaDon);
        }

        // Hàm hỗ trợ GetCart
        private List<CartItemViewModel> GetCart()
        {
            var sessionData = _httpContextAccessor.HttpContext?.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(sessionData)) return new List<CartItemViewModel>();
            try
            {
                return JsonSerializer.Deserialize<List<CartItemViewModel>>(sessionData) ?? new List<CartItemViewModel>();
            }
            catch { return new List<CartItemViewModel>(); }
        }
    }
}