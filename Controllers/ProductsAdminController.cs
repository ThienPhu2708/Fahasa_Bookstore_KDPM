using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using CNPM_LIBRARY_MANAGEMENT.Models;
using Microsoft.AspNetCore.Hosting; // <-- Cần cho Upload Ảnh
using System.IO; // <-- Cần cho Upload Ảnh

namespace CNPM_LIBRARY_MANAGEMENT.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class ProductsAdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // <-- Dùng để lấy đường dẫn wwwroot

        public ProductsAdminController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: ProductsAdmin (Trang danh sách)
        public async Task<IActionResult> Index(string searchString, string type)
        {

            // 1. Khởi tạo truy vấn cơ bản và nạp bảng liên quan
            var query = _context.SanPhams
                .Include(s => s.LoaiSanPham)
                .AsQueryable();

            // 2. Lọc theo từ khóa tìm kiếm (Tên sản phẩm)
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.TenSanPham.Contains(searchString));
            }

            // 3. Lọc theo Loại (Sách hoặc Văn phòng phẩm)
            if (!string.IsNullOrEmpty(type))
            {
                // Lưu ý: s.LoaiSanPham.TenLoaiSp phải khớp với tên trong DB của bạn
                query = query.Where(s => s.LoaiSanPham.TenLoaiSp.Contains(type));
            }

            // 4. Lưu lại giá trị để hiển thị trên Form
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentType = type;

            // 5. Trả về View
            return View(await query.ToListAsync());
        }

        // GET: ProductsAdmin/Create
        // Action này sẽ hiển thị một trang "Chọn loại"
        public IActionResult Create()
        {
            return View();
        }

        // GET: ProductsAdmin/CreateProduct?typeId=2
        // Action này hiển thị form tạo Sách (typeId=2) hoặc VPP (typeId=1)
        public async Task<IActionResult> CreateProduct(int typeId)
        {
            if (typeId != 1 && typeId != 2)
            {
                return RedirectToAction("Create"); // Quay lại trang chọn
            }

            var viewModel = new ProductAdminViewModel
            {
                LoaiSanPhamId = typeId
            };

            // Tải tất cả các danh sách dropdown
            await PopulateDropdowns(viewModel);

            return View(viewModel);
        }

        // HÀM QUAN TRỌNG: Tải dữ liệu cho các Dropdown
        private async Task PopulateDropdowns(ProductAdminViewModel viewModel)
        {
            // 1. Tải cho Sách
            viewModel.TacGias = await _context.TacGia
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.TenTacGia })
                .ToListAsync();

            viewModel.TheLoaiSaches = await _context.TheLoaiSaches
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.TenTheLoai })
                .ToListAsync();

            viewModel.NhaXuatBans = await _context.NhaXuatBans
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.TenNxb })
                .ToListAsync();

            // 2. Tải cho VPP
            viewModel.LoaiVpphams = await _context.LoaiVpphams
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.TenLoaiVpp })
                .ToListAsync();

            viewModel.ThuongHieus = await _context.ThuongHieus
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.TenThuongHieu })
                .ToListAsync();

            viewModel.NhaSanXuats = await _context.NhaSanXuats
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.TenNsx })
                .ToListAsync();

            viewModel.AllMauSacs = await _context.MauSacs
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.TenMauSac })
                .ToListAsync();

            viewModel.AllChatLieus = await _context.ChatLieus
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.TenChatLieu })
                .ToListAsync();
        }

        // POST: ProductsAdmin/CreateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductAdminViewModel viewModel)
        {
            if (viewModel.LoaiSanPhamId == 2) 
            {
                if (!viewModel.TacGiaId.HasValue)
                    ModelState.AddModelError("TacGiaId", "Vui lòng chọn Tác giả");
                if (!viewModel.TheLoaiId.HasValue) 
                    ModelState.AddModelError("TheLoaiId", "Vui lòng chọn Thể loại");

                ModelState.Remove("IdLoaiVpp");
                ModelState.Remove("ThuongHieuId");
                ModelState.Remove("NhaSanXuatId");
            }
            else if (viewModel.LoaiSanPhamId == 1) // Nếu là VPP
            {
                if (!viewModel.IdLoaiVpp.HasValue) 
                    ModelState.AddModelError("IdLoaiVpp", "Vui lòng chọn Loại VPP");

                ModelState.Remove("TacGiaId");
                ModelState.Remove("TheLoaiId");
                ModelState.Remove("NhaXuatBanId");
                ModelState.Remove("SoTrang");
                ModelState.Remove("LoaiBia");
            }

            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        string tenFileAnh = "default_product_img.jpg";
                        if (viewModel.ImageFile != null)
                        {
                            tenFileAnh = await UploadFile(viewModel.ImageFile);
                        }

                        var sanPham = new SanPham
                        {
                            TenSanPham = viewModel.TenSanPham,
                            GiaBan = viewModel.GiaBan,
                            MoTa = viewModel.MoTa,
                            HinhAnh = tenFileAnh,
                            LoaiSanPhamId = viewModel.LoaiSanPhamId,
                            SoLuongDaBan = 0,
                            DanhGia = 5,
                            PhanTramGiam = viewModel.PhanTramGiam,
                            HangMoiVe = viewModel.HangMoiVe
                        };
                        _context.SanPhams.Add(sanPham);
                        await _context.SaveChangesAsync();

                        if (viewModel.LoaiSanPhamId == 2) // Sách
                        {
                            var chiTietSach = new ChiTietSach
                            {
                                SanPhamId = sanPham.Id,
                                TacGiaId = viewModel.TacGiaId,
                                TheLoaiId = viewModel.TheLoaiId.Value, 
                                NhaXuatBanId = viewModel.NhaXuatBanId,
                                SoTrang = viewModel.SoTrang,
                                LoaiBia = viewModel.LoaiBia
                            };
                            _context.ChiTietSaches.Add(chiTietSach);
                        }
                        else if (viewModel.LoaiSanPhamId == 1) // VPP
                        {
                            var chiTietVpp = new ChiTietVpp
                            {
                                SanPhamId = sanPham.Id,
                                IdloaiVpp = viewModel.IdLoaiVpp.Value,
                                ThuongHieuId = viewModel.ThuongHieuId,
                                NhaSanXuatId = viewModel.NhaSanXuatId
                            };

                            if (viewModel.SelectedMauSacIds != null && viewModel.SelectedMauSacIds.Any())
                            {
                                var selectedColors = await _context.MauSacs
                                    .Where(m => viewModel.SelectedMauSacIds.Contains(m.Id))
                                    .ToListAsync();

                                chiTietVpp.MauSacs = selectedColors;
                            }

                            if (viewModel.SelectedChatLieuIds != null && viewModel.SelectedChatLieuIds.Any())
                            {
                                var selectedMaterials = await _context.ChatLieus
                                    .Where(c => viewModel.SelectedChatLieuIds.Contains(c.Id))
                                    .ToListAsync();
                                chiTietVpp.ChatLieus = selectedMaterials;
                            }

                            _context.ChiTietVpps.Add(chiTietVpp);   
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi tạo sản phẩm: " + ex.Message);
                    }
                }
            }

            // Nếu ModelState không hợp lệ, tải lại Dropdowns và trả về View
            await PopulateDropdowns(viewModel);
            return View("CreateProduct", viewModel);
        }

        // HÀM HỖ TRỢ UPLOAD FILE
        private async Task<string> UploadFile(IFormFile file)
        {
            // 1. Tạo tên file độc nhất (unique)
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            // 2. Lấy đường dẫn tuyệt đối đến thư mục `wwwroot/images/product-images`
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/product-images");

            // 3. Tạo đường dẫn file đầy đủ
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // 4. Đảm bảo thư mục tồn tại
            Directory.CreateDirectory(uploadsFolder);

            // 5. Lưu file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 6. Trả về tên file để lưu vào DB
            return uniqueFileName;
        }

        // GET: ProductsAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Lấy SanPham, Include cả ChiTietSach và ChiTietVPP
            var sanPham = await _context.SanPhams
                .Include(s => s.LoaiSanPham)
                // Sách
                .Include(s => s.ChiTietSach).ThenInclude(cts => cts.TacGia)
                .Include(s => s.ChiTietSach).ThenInclude(cts => cts.TheLoai)
                .Include(s => s.ChiTietSach).ThenInclude(cts => cts.NhaXuatBan)
                // VPP
                .Include(s => s.ChiTietVpp).ThenInclude(ctv => ctv.IdloaiVppNavigation)
                .Include(s => s.ChiTietVpp).ThenInclude(ctv => ctv.ThuongHieu)
                .Include(s => s.ChiTietVpp).ThenInclude(ctv => ctv.NhaSanXuat)
                .AsNoTracking() 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        // GET: ProductsAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var sanPham = await _context.SanPhams
                .Include(s => s.ChiTietSach)
                .Include(s => s.ChiTietVpp) 
                    .ThenInclude(v => v.MauSacs) 
                .Include(s => s.ChiTietVpp) 
                    .ThenInclude(v => v.ChatLieus)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sanPham == null) return NotFound();

            // --- Map từ Model CSDL sang ViewModel ---
            var viewModel = new ProductAdminViewModel
            {
                Id = sanPham.Id,
                TenSanPham = sanPham.TenSanPham,
                GiaBan = sanPham.GiaBan,
                MoTa = sanPham.MoTa,
                HinhAnh = sanPham.HinhAnh,
                LoaiSanPhamId = sanPham.LoaiSanPhamId,
                PhanTramGiam = sanPham.PhanTramGiam,
                HangMoiVe = sanPham.HangMoiVe
            };

            // Load chi tiết Sách (nếu có)
            if (sanPham.LoaiSanPhamId == 2 && sanPham.ChiTietSach != null)
            {
                viewModel.TacGiaId = sanPham.ChiTietSach.TacGiaId;
                viewModel.TheLoaiId = sanPham.ChiTietSach.TheLoaiId;
                viewModel.NhaXuatBanId = sanPham.ChiTietSach.NhaXuatBanId;
                viewModel.SoTrang = sanPham.ChiTietSach.SoTrang;
                viewModel.LoaiBia = sanPham.ChiTietSach.LoaiBia;
            }
            // Load chi tiết VPP (nếu có)
            else if (sanPham.LoaiSanPhamId == 1 && sanPham.ChiTietVpp != null)
            {
                viewModel.IdLoaiVpp = sanPham.ChiTietVpp.IdloaiVpp;
                viewModel.ThuongHieuId = sanPham.ChiTietVpp.ThuongHieuId;
                viewModel.NhaSanXuatId = sanPham.ChiTietVpp.NhaSanXuatId;
                viewModel.SelectedMauSacIds = sanPham.ChiTietVpp.MauSacs.Select(m => m.Id).ToList();
            }

            // Tải Dropdowns
            await PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        // POST: ProductsAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductAdminViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            // Validate tương tự như Create
            if (viewModel.LoaiSanPhamId == 2) // Sách
            {
                if (!viewModel.TacGiaId.HasValue) ModelState.AddModelError("TacGiaId", "Vui lòng chọn Tác giả");
                if (!viewModel.TheLoaiId.HasValue) ModelState.AddModelError("TheLoaiId", "Vui lòng chọn Thể loại");
                ModelState.Remove("IdLoaiVpp");
            }
            else if (viewModel.LoaiSanPhamId == 1) // VPP
            {
                if (!viewModel.IdLoaiVpp.HasValue) ModelState.AddModelError("IdLoaiVpp", "Vui lòng chọn Loại VPP");
                ModelState.Remove("TacGiaId");
                ModelState.Remove("TheLoaiId");
            }

            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var sanPham = await _context.SanPhams
                            .Include(s => s.ChiTietSach)
                            .Include(s => s.ChiTietVpp) 
                                .ThenInclude(v => v.MauSacs) 
                            .Include(s => s.ChiTietVpp) 
                                .ThenInclude(v => v.ChatLieus) 
                            .FirstOrDefaultAsync(s => s.Id == id);

                        if (sanPham == null) return NotFound();

                        // --- Bước 2: Xử lý file ảnh (nếu có file mới) ---
                        if (viewModel.ImageFile != null)
                        {
                            // (Nâng cao: nên xóa file ảnh cũ)
                            sanPham.HinhAnh = await UploadFile(viewModel.ImageFile);
                        }

                        // --- Bước 3: Cập nhật Bảng Cha (SanPham) ---
                        sanPham.TenSanPham = viewModel.TenSanPham;
                        sanPham.GiaBan = viewModel.GiaBan;
                        sanPham.MoTa = viewModel.MoTa;
                        sanPham.PhanTramGiam = viewModel.PhanTramGiam;
                        sanPham.HangMoiVe = viewModel.HangMoiVe;
                        // Không cho đổi LoaiSanPhamId khi Edit

                        _context.SanPhams.Update(sanPham);

                        // --- Bước 4: Cập nhật Bảng Con (ChiTietSach / ChiTietVpp) ---
                        if (sanPham.LoaiSanPhamId == 2 && sanPham.ChiTietSach != null)
                        {
                            sanPham.ChiTietSach.TacGiaId = viewModel.TacGiaId;
                            sanPham.ChiTietSach.TheLoaiId = viewModel.TheLoaiId.Value;
                            sanPham.ChiTietSach.NhaXuatBanId = viewModel.NhaXuatBanId;
                            sanPham.ChiTietSach.SoTrang = viewModel.SoTrang;
                            sanPham.ChiTietSach.LoaiBia = viewModel.LoaiBia;
                            _context.ChiTietSaches.Update(sanPham.ChiTietSach);
                        }
                        else if (sanPham.LoaiSanPhamId == 1 && sanPham.ChiTietVpp != null)
                        {
                            sanPham.ChiTietVpp.IdloaiVpp = viewModel.IdLoaiVpp.Value;
                            sanPham.ChiTietVpp.ThuongHieuId = viewModel.ThuongHieuId;
                            sanPham.ChiTietVpp.NhaSanXuatId = viewModel.NhaSanXuatId;

                            sanPham.ChiTietVpp.MauSacs.Clear(); 
                            if (viewModel.SelectedMauSacIds != null && viewModel.SelectedMauSacIds.Any())
                            {
                                var selectedColors = await _context.MauSacs
                                    .Where(m => viewModel.SelectedMauSacIds.Contains(m.Id))
                                    .ToListAsync();
                                sanPham.ChiTietVpp.MauSacs = selectedColors;
                            }

                            sanPham.ChiTietVpp.ChatLieus.Clear();
                            if (viewModel.SelectedChatLieuIds != null && viewModel.SelectedChatLieuIds.Any())
                            {
                                var selectedMaterials = await _context.ChatLieus
                                    .Where(c => viewModel.SelectedChatLieuIds.Contains(c.Id))
                                    .ToListAsync();
                                sanPham.ChiTietVpp.ChatLieus = selectedMaterials;
                            }
                            _context.ChiTietVpps.Update(sanPham.ChiTietVpp);
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                    }
                }
            }

            // Nếu lỗi, tải lại dropdowns và trả về
            await PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        // GET: ProductsAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                .Include(s => s.LoaiSanPham)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        // POST: ProductsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var sanPham = await _context.SanPhams
                        .Include(s => s.ChiTietSach)
                        .Include(s => s.ChiTietVpp)
                        .FirstOrDefaultAsync(s => s.Id == id);

                    if (sanPham == null) return NotFound();

                    // --- Bước 1: Xóa Bảng Con (RẤT QUAN TRỌNG) ---
                    // Phải xóa chi tiết trước do ràng buộc khóa ngoại
                    if (sanPham.ChiTietSach != null)
                    {
                        _context.ChiTietSaches.Remove(sanPham.ChiTietSach);
                    }
                    if (sanPham.ChiTietVpp != null)
                    {
                        _context.ChiTietVpps.Remove(sanPham.ChiTietVpp);
                    }
                    // (Lưu ý: Bảng ChiTietHoaDon cũng có khóa ngoại, 
                    // nhưng ta sẽ set ON DELETE SET NULL hoặc CASCADE trong CSDL)
                    // (Tạm thời, nếu sản phẩm đã có trong hóa đơn, code này sẽ lỗi.
                    // Chúng ta sẽ xử lý sau bằng cách "ẩn" sản phẩm thay vì xóa)

                    await _context.SaveChangesAsync(); // Lưu thay đổi xóa bảng con

                    // --- Bước 2: Xóa Bảng Cha (SanPham) ---
                    _context.SanPhams.Remove(sanPham);

                    // (Nên xóa file ảnh trong wwwroot ở đây)

                    await _context.SaveChangesAsync(); // Lưu thay đổi xóa bảng cha

                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Nếu lỗi (ví dụ: đã có trong hóa đơn), quay lại trang Index
                    return RedirectToAction(nameof(Index));
                }
            }
        }
    }
}