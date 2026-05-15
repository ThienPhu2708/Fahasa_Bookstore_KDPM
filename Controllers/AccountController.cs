using System;
using System.Collections.Generic;
using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using CNPM_LIBRARY_MANAGEMENT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using CNPM_LIBRARY_MANAGEMENT.Helpers;

namespace CNPM_LIBRARY_MANAGEMENT.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // === SỬA LẠI: Dùng Gmail ===
                if (await _context.Accounts.AnyAsync(a => a.Gmail == viewModel.Gmail))
                {
                    ModelState.AddModelError("Gmail", "Địa chỉ Email này đã được sử dụng.");
                    return View(viewModel);
                }

                PasswordHelper.CreatePasswordHash(viewModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var account = new Account
                {
                    Gmail = viewModel.Gmail,
                    TenNguoiDung = viewModel.TenNguoiDung,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = "User",
                    NgayTao = DateTime.Now
                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Gmail == viewModel.Gmail);

                if (account == null || !PasswordHelper.VerifyPasswordHash(viewModel.Password, account.PasswordHash, account.PasswordSalt))
                {
                    ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
                    return View(viewModel);
                }

                HttpContext.Session.SetString("UserId", account.Id.ToString());
                HttpContext.Session.SetString("Username", account.TenNguoiDung ?? account.Gmail);
                HttpContext.Session.SetString("UserRole", account.Role);

                // Merge giỏ hàng session (guest) với giỏ hàng đã lưu trong DB
                var cartController = new CartController(_context, _httpContextAccessor);
                await cartController.LoadCartFromDbAsync(account.Id);

                if (account.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Product");
                }
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Logout()
        {
            // Lưu giỏ hàng hiện tại vào DB trước khi xóa session
            var cartController = new CartController(_context, _httpContextAccessor);
            var cart = cartController.GetCartPublic();
            if (cart.Any())
            {
                await cartController.SyncCartToDbPublicAsync(cart);
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // === PROFILE ===
        public async Task<IActionResult> Profile()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login");
            if (!int.TryParse(userIdString, out int userId)) return RedirectToAction("Login");

            var account = await _context.Accounts.FindAsync(userId);
            if (account == null) return RedirectToAction("Login");

            // Trả về Views/Account/Profile.cshtml
            return View("Profile", account);
        }

        // === EDIT PROFILE (GET) ===
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login");
            if (!int.TryParse(userIdString, out int userId)) return RedirectToAction("Login");

            var account = await _context.Accounts.FindAsync(userId);
            if (account == null) return RedirectToAction("Login");

            var viewModel = new EditProfileViewModel
            {
                Id = account.Id,
                Gmail = account.Gmail,
                TenNguoiDung = account.TenNguoiDung,
                GioiThieu = account.GioiThieu,
                CurrentAvatar = account.AvatarNguoiDung
            };
            return View(viewModel);
        }

        // === EDIT PROFILE (POST) ===
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel viewModel)
        {
            if (viewModel.Id == 0) return NotFound();

            ModelState.Remove("AvatarFile");
            ModelState.Remove("Gmail");

            if (ModelState.IsValid)
            {
                try
                {
                    var accountToUpdate = await _context.Accounts.FindAsync(viewModel.Id);
                    if (accountToUpdate == null) return NotFound();

                    string uniqueFileName = accountToUpdate.AvatarNguoiDung;

                    if (viewModel.AvatarFile != null)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");

                        if (!string.IsNullOrEmpty(accountToUpdate.AvatarNguoiDung) && accountToUpdate.AvatarNguoiDung != "default_avatar.jpg")
                        {
                            string oldFilePath = Path.Combine(uploadsFolder, accountToUpdate.AvatarNguoiDung);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewModel.AvatarFile.FileName);
                        string newFilePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await viewModel.AvatarFile.CopyToAsync(fileStream);
                        }
                    }

                    accountToUpdate.TenNguoiDung = viewModel.TenNguoiDung;
                    accountToUpdate.GioiThieu = viewModel.GioiThieu;
                    accountToUpdate.AvatarNguoiDung = uniqueFileName;

                    _context.Update(accountToUpdate);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("Username", accountToUpdate.TenNguoiDung ?? accountToUpdate.Gmail);
                    return RedirectToAction("Profile");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(viewModel.Id)) return NotFound();
                    else throw;
                }
            }
            return View(viewModel);
        }

        // === ORDER HISTORY ===
        public async Task<IActionResult> OrderHistory()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int accountId))
            {
                return RedirectToAction("Login");
            }
            var userOrders = await _context.HoaDons
                .Where(h => h.AccountId == accountId)
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPham)
                .OrderByDescending(h => h.NgayDatHang)
                .ToListAsync();
            return View(userOrders);
        }

        // === CHANGE PASSWORD (GET) ===
        public IActionResult ChangePassword()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        // === CHANGE PASSWORD (POST) ===
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int accountId))
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                var account = await _context.Accounts.FindAsync(accountId);
                if (account == null)
                {
                    return RedirectToAction("Login");
                }

                if (!PasswordHelper.VerifyPasswordHash(model.OldPassword, account.PasswordHash, account.PasswordSalt))
                {
                    ModelState.AddModelError("OldPassword", "Mật khẩu cũ không chính xác.");
                    return View(model);
                }

                PasswordHelper.CreatePasswordHash(model.NewPassword, out byte[] newHash, out byte[] newSalt);
                account.PasswordHash = newHash;
                account.PasswordSalt = newSalt;

                _context.Update(account);
                await _context.SaveChangesAsync();

                return RedirectToAction("Profile");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // 1. XỬ LÝ QUÊN MẬT KHẨU (Gửi mã giả lập)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra email có trong DB không
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Gmail == model.Email);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "Email không tồn tại trong hệ thống.";
                    return View(model);
                }

                // --- GIẢ LẬP GỬI EMAIL ---
                // 1. Tạo mã ngẫu nhiên 6 số
                string otpCode = new Random().Next(100000, 999999).ToString();

                // 2. Lưu mã này vào Session để lát nữa kiểm tra (Kèm email để đảm bảo đúng người)
                HttpContext.Session.SetString("ResetOtp", otpCode);
                HttpContext.Session.SetString("ResetEmail", model.Email);

                // 3. Hiển thị mã ra màn hình (Thay vì gửi email)
                TempData["SuccessMessage"] = $"[DEMO] Mã xác nhận của bạn là: {otpCode}"; // <--- QUAN TRỌNG

                // Chuyển sang trang nhập mật khẩu mới
                return RedirectToAction("ResetPassword");
            }

            return View(model);
        }

        // 2. HIỂN THỊ TRANG ĐẶT LẠI MẬT KHẨU
        [HttpGet]
        public IActionResult ResetPassword()
        {
            // Lấy email từ session để điền sẵn cho tiện
            var email = HttpContext.Session.GetString("ResetEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword");
            }

            return View(new ResetPasswordViewModel { Email = email });
        }

        // 3. XỬ LÝ ĐỔI MẬT KHẨU MỚI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Lấy mã OTP và Email từ Session ra so sánh
                var sessionOtp = HttpContext.Session.GetString("ResetOtp");
                var sessionEmail = HttpContext.Session.GetString("ResetEmail");

                // 2. Kiểm tra khớp dữ liệu
                if (sessionOtp != model.Token || sessionEmail != model.Email)
                {
                    ModelState.AddModelError("Token", "Mã xác nhận không đúng hoặc đã hết hạn.");
                    return View(model);
                }

                // 3. Tìm user trong DB để đổi mật khẩu
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Gmail == model.Email);
                if (user != null)
                {
                    // --- TẠO PASSWORD HASH & SALT MỚI ---
                    CreatePasswordHash(model.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;

                    _context.Accounts.Update(user);
                    await _context.SaveChangesAsync();

                    // 4. Xóa Session OTP
                    HttpContext.Session.Remove("ResetOtp");
                    HttpContext.Session.Remove("ResetEmail");

                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công! Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }

        // === API: ĐĂNG NHẬP (dùng cho Postman / kiểm thử API) ===
        // POST /Account/ApiLogin
        // Body (JSON): { "gmail": "user@example.com", "password": "123456" }
        [HttpPost]
        public async Task<IActionResult> ApiLogin([FromBody] LoginViewModel viewModel)
        {
            if (viewModel == null)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Gmail == viewModel.Gmail);

            if (account == null || !PasswordHelper.VerifyPasswordHash(viewModel.Password, account.PasswordHash, account.PasswordSalt))
            {
                return Ok(new { success = false, message = "Email hoặc mật khẩu không đúng." });
            }

            HttpContext.Session.SetString("UserId", account.Id.ToString());
            HttpContext.Session.SetString("Username", account.TenNguoiDung ?? account.Gmail);
            HttpContext.Session.SetString("UserRole", account.Role);

            return Ok(new { success = true, message = "Đăng nhập thành công.", userId = account.Id, role = account.Role });
        }
    }
}