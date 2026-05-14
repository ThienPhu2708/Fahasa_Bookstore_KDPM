using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Bắt buộc có để dùng Session
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CNPM_LIBRARY_MANAGEMENT.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        // 1. KHAI BÁO BIẾN ĐỂ DÙNG SESSION
        private readonly IHttpContextAccessor _httpContextAccessor;

        // 2. TIÊM VÀO CONSTRUCTOR (Dependency Injection)
        public ProductController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor; // Gán giá trị
        }

        // Action Index (Hiển thị danh sách, có lọc)
        public async Task<IActionResult> Index(
            int? categoryId,
            int? subCategoryId,
            string? query,
            decimal? minPrice,        // <--- Tham số mới: Giá tối thiểu
            decimal? maxPrice,        // <--- Tham số mới: Giá tối đa
            string? sortBy)           // <--- Tham số mới: Sắp xếp
        {
            // 1. Bắt đầu truy vấn
            var queryable = _context.SanPhams.AsQueryable();

            // 2. Lọc theo Tên sản phẩm
            if (!string.IsNullOrEmpty(query))
            {
                queryable = queryable.Where(p => p.TenSanPham.Contains(query));
            }

            // 3. Lọc theo Loại sản phẩm & Loại con (logic cũ)
            if (categoryId.HasValue && categoryId > 0)
            {
                queryable = queryable.Where(p => p.LoaiSanPhamId == categoryId);

                if (subCategoryId.HasValue && subCategoryId > 0)
                {
                    if (categoryId == 1) // VPP
                    {
                        queryable = queryable.Where(p => p.ChiTietVpp.IdloaiVpp == subCategoryId);
                    }
                    else if (categoryId == 2) // Sách
                    {
                        queryable = queryable.Where(p => p.ChiTietSach.TheLoaiId == subCategoryId);
                    }
                }
            }

            // =========================================================
            // 4. LỌC THEO GIÁ 
            // =========================================================
            if (minPrice.HasValue)
            {
                queryable = queryable.Where(p => p.GiaBan >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                queryable = queryable.Where(p => p.GiaBan <= maxPrice.Value);
            }

            // =========================================================
            // 5. SẮP XẾP 
            // =========================================================
            switch (sortBy?.ToLower())
            {
                case "price_asc": // Giá tăng dần
                    queryable = queryable.OrderBy(p => p.GiaBan);
                    break;
                case "price_desc": // Giá giảm dần
                    queryable = queryable.OrderByDescending(p => p.GiaBan);
                    break;
                case "date_desc": // Mới nhất
                    queryable = queryable.OrderByDescending(p => p.NgayDang);
                    break;
                case "rating_desc": // Đánh giá cao nhất
                    queryable = queryable.OrderByDescending(p => p.DanhGia);
                    break;
                default: // Mặc định sắp xếp theo ID
                    queryable = queryable.OrderBy(p => p.Id);
                    break;
            }


            // 6. Thêm Includes (Giữ nguyên)
            var finalQuery = queryable
                .Include(p => p.ChiTietSach).ThenInclude(cts => cts.TheLoai)
                .Include(p => p.ChiTietSach).ThenInclude(cts => cts.TacGia)
                .Include(p => p.ChiTietVpp).ThenInclude(ctv => ctv.IdloaiVppNavigation)
                .Include(p => p.ChiTietVpp).ThenInclude(ctv => ctv.ThuongHieu);

            // 7. Thực thi truy vấn
            var products = await finalQuery.ToListAsync();

            // 8. Trả về View (Lưu lại các filter để giữ trạng thái trên giao diện)
            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.CurrentQuery = query;
            ViewBag.CurrentMinPrice = minPrice; 
            ViewBag.CurrentMaxPrice = maxPrice; 
            ViewBag.CurrentSortBy = sortBy;     

            var userWishlistIds = new HashSet<int>(); 
            var currentUserIdStr = _httpContextAccessor.HttpContext?.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(currentUserIdStr) && int.TryParse(currentUserIdStr, out int currentAccId))
            {
                var listIds = await _context.YeuThichs
                    .Where(y => y.AccountId == currentAccId)
                    .Select(y => y.SanPhamId)
                    .ToListAsync(); 

                userWishlistIds = new HashSet<int>(listIds);
            }

            // Truyền danh sách này sang View
            ViewBag.UserWishlistIds = userWishlistIds;
            // ==================================================

            return View(products);
        }

        // Action Details (Chi tiết 1 sản phẩm)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }

            var sanPham = await _context.SanPhams
                .Where(p => p.Id == id)
                .Include(p => p.ChiTietSach).ThenInclude(cts => cts.TheLoai)
                .Include(p => p.ChiTietSach).ThenInclude(cts => cts.TacGia)
                .Include(p => p.ChiTietSach).ThenInclude(cts => cts.NhaXuatBan)
                .Include(p => p.ChiTietVpp).ThenInclude(ctv => ctv.IdloaiVppNavigation)
                .Include(p => p.ChiTietVpp).ThenInclude(ctv => ctv.ThuongHieu)
                .Include(p => p.ChiTietVpp).ThenInclude(ctv => ctv.NhaSanXuat)
                .Include(p => p.ChiTietVpp).ThenInclude(ctv => ctv.MauSacs)
                .Include(p => p.ChiTietVpp).ThenInclude(ctv => ctv.ChatLieus)

                // --- QUAN TRỌNG: LOAD DANH SÁCH ĐÁNH GIÁ ĐỂ HIỂN THỊ ---
                .Include(p => p.DanhGias).ThenInclude(dg => dg.Account)

                .FirstOrDefaultAsync();

            if (sanPham == null) { return NotFound(); }

            // --- TÌM SẢN PHẨM GỢI Ý (RELATED PRODUCTS) ---
            var relatedQuery = _context.SanPhams.AsQueryable();

            if (sanPham.LoaiSanPhamId == 2 && sanPham.ChiTietSach != null)
            {
                var theLoaiId = sanPham.ChiTietSach.TheLoaiId;
                relatedQuery = relatedQuery.Where(p => p.LoaiSanPhamId == 2
                                                    && p.ChiTietSach.TheLoaiId == theLoaiId);
            }
            else if (sanPham.LoaiSanPhamId == 1 && sanPham.ChiTietVpp != null)
            {
                var loaiVppId = sanPham.ChiTietVpp.IdloaiVpp;
                relatedQuery = relatedQuery.Where(p => p.LoaiSanPhamId == 1
                                                    && p.ChiTietVpp.IdloaiVpp == loaiVppId);
            }

            ViewBag.RelatedProducts = await relatedQuery
                .Where(p => p.Id != id)
                .OrderBy(r => Guid.NewGuid())
                .Take(5)
                .ToListAsync();

            bool isWishlisted = false;
            var userIdStr = _httpContextAccessor.HttpContext?.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out int uId))
            {
                // Kiểm tra trong bảng YeuThich xem có cặp (AccountId, SanPhamId) này không
                isWishlisted = await _context.YeuThichs
                    .AnyAsync(y => y.AccountId == uId && y.SanPhamId == sanPham.Id);
            }

            ViewBag.IsWishlisted = isWishlisted;

            return View(sanPham);
        }

        // GET: /Product/Search
        [HttpGet]
        public IActionResult Search(string? query)
        {
            string? categoryId = Request.Query["categoryId"];
            string? subCategoryId = Request.Query["subCategoryId"];

            return RedirectToAction("Index", new
            {
                query = query,
                categoryId = categoryId,
                subCategoryId = subCategoryId
            });
        }

        // Action trang Khuyến mãi
        public async Task<IActionResult> KhuyenMai(string? sortBy)
        {
            var queryable = _context.SanPhams
                .Where(p => p.PhanTramGiam != null && p.PhanTramGiam > 0)
                .Include(p => p.ChiTietSach).ThenInclude(cts => cts.TacGia)
                .Include(p => p.ChiTietVpp).ThenInclude(ctv => ctv.ThuongHieu)
                .AsQueryable();

            queryable = sortBy switch
            {
                "price_asc" => queryable.OrderBy(p => p.GiaBan * (1 - p.PhanTramGiam!.Value / 100)),
                "price_desc" => queryable.OrderByDescending(p => p.GiaBan * (1 - p.PhanTramGiam!.Value / 100)),
                "discount_desc" => queryable.OrderByDescending(p => p.PhanTramGiam),
                _ => queryable.OrderByDescending(p => p.PhanTramGiam)
            };

            ViewBag.CurrentSortBy = sortBy;
            var products = await queryable.ToListAsync();
            return View(products);
        }

        // Action trang Hàng mới về
        public async Task<IActionResult> HangMoiVe(string? sortBy)
        {
            var queryable = _context.SanPhams
                .Where(p => p.HangMoiVe)
                .Include(p => p.ChiTietSach).ThenInclude(cts => cts.TacGia)
                .Include(p => p.ChiTietVpp).ThenInclude(ctv => ctv.ThuongHieu)
                .AsQueryable();

            queryable = sortBy switch
            {
                "price_asc" => queryable.OrderBy(p => p.GiaBan),
                "price_desc" => queryable.OrderByDescending(p => p.GiaBan),
                "date_desc" => queryable.OrderByDescending(p => p.NgayDang),
                _ => queryable.OrderByDescending(p => p.Id)
            };

            ViewBag.CurrentSortBy = sortBy;
            var products = await queryable.ToListAsync();
            return View(products);
        }

        // Action Thêm Bình Luận
        [HttpPost]
        public async Task<IActionResult> AddComment(int sanPhamId, string noiDung, int soSao)
        {
            // 1. Kiểm tra đăng nhập (Dùng biến _httpContextAccessor đã khai báo ở trên)
            var userIdString = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int accountId))
            {
                // Trả về login nếu chưa đăng nhập
                return RedirectToAction("Login", "Account", new { returnUrl = $"/Product/Details/{sanPhamId}" });
            }

            // 2. Lưu đánh giá mới
            var danhGia = new DanhGia
            {
                SanPhamId = sanPhamId,
                AccountId = accountId,
                NoiDung = noiDung,

                // --- SỬA LỖI: ÉP KIỂU TỪ INT SANG BYTE ---
                Sao = (byte)soSao,

                NgayTao = DateTime.Now
            };

            _context.DanhGias.Add(danhGia);
            await _context.SaveChangesAsync();

            // 3. TÍNH LẠI ĐIỂM TRUNG BÌNH CHO SẢN PHẨM
            var sanPham = await _context.SanPhams.FindAsync(sanPhamId);
            if (sanPham != null)
            {
                var listDanhGia = _context.DanhGias.Where(d => d.SanPhamId == sanPhamId).ToList();

                if (listDanhGia.Any())
                {
                    // Hàm Average trả về double (ví dụ 4.5)
                    double diemTrungBinh = listDanhGia.Average(d => d.Sao);

                    // --- SỬA LỖI: LÀM TRÒN VÀ ÉP KIỂU SANG BYTE ---
                    sanPham.DanhGia = (byte?)Math.Round(diemTrungBinh);
                }
                else
                {
                    // Ép kiểu sang byte
                    sanPham.DanhGia = (byte)soSao;
                }

                _context.SanPhams.Update(sanPham);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = sanPhamId });
        }
    }
}