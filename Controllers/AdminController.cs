    using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.IO;

namespace CNPM_LIBRARY_MANAGEMENT.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        // 1. Tiêm AppDbContext
        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // 2. Cập nhật Index (GET) để tải đơn hàng "Chờ xử lý"
        public async Task<IActionResult> Index()
        {
            var pendingOrders = await _context.HoaDons
                .Where(h => h.TrangThai == "Chờ xử lý")
                .Include(h => h.Account) 
                .OrderByDescending(h => h.NgayDatHang)
                .ToListAsync();

            return View(pendingOrders);
        }

        // 3. Action để xử lý nút Duyệt / Hủy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _context.HoaDons.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái
            order.TrangThai = newStatus;

            // (Nâng cao: Nếu newStatus == "Đã hủy", bạn có thể thêm logic
            // để hoàn trả số lượng sản phẩm vào kho)

            _context.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // 4. (MỚI) Action để xem Chi tiết Đơn hàng
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.Account) 
                .Include(h => h.ChiTietHoaDons) 
                    .ThenInclude(ct => ct.SanPham) 
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // GET: /Admin/ExportToExcel
        public async Task<IActionResult> ExportToExcel()
        {
            // 1. Bảo vệ
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            // 2. Lấy dữ liệu cần xuất
            var approvedOrders = await _context.HoaDons
                .Where(h => h.TrangThai == "Đang giao" || h.TrangThai == "Đã hoàn thành") 
                .Include(h => h.Account)
                .OrderByDescending(h => h.NgayDatHang)
                .ToListAsync();

            // 3. Tạo file Excel (Giữ nguyên logic V1)
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoCaoDoanhThu");
                var currentRow = 1;

                // 4. Thêm Tiêu đề (Header)
                worksheet.Cell(currentRow, 1).Value = "Mã ĐH";
                worksheet.Cell(currentRow, 2).Value = "Người đặt";
                worksheet.Cell(currentRow, 3).Value = "Email";
                worksheet.Cell(currentRow, 4).Value = "Ngày đặt";
                worksheet.Cell(currentRow, 5).Value = "Trạng thái";
                worksheet.Cell(currentRow, 6).Value = "Tổng tiền";

                worksheet.Row(currentRow).Style.Font.Bold = true;

                // 5. Thêm Dữ liệu (Data)
                foreach (var order in approvedOrders)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = order.Id;
                    worksheet.Cell(currentRow, 2).Value = order.Account?.TenNguoiDung;

                    worksheet.Cell(currentRow, 3).Value = order.Account?.Gmail;

                    worksheet.Cell(currentRow, 4).Value = order.NgayDatHang;
                    worksheet.Cell(currentRow, 5).Value = order.TrangThai;
                    worksheet.Cell(currentRow, 6).Value = order.TongTien;

                    worksheet.Cell(currentRow, 4).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "#,##0 ₫";
                }

                worksheet.Columns().AdjustToContents();

                // 6. Lưu file vào bộ nhớ và Trả về
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string excelName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }
        }

        // 5. Action AJAX để cung cấp dữ liệu Doanh thu hàng ngày cho biểu đồ
        [HttpGet]
        public async Task<IActionResult> GetDailyRevenueData()
        {   
            var startDate = DateTime.Today.AddDays(-30);

            // BƯỚC 1: Lấy danh sách đơn hàng thô về RAM trước (Không GroupBy ở đây để tránh lỗi SQL)
            var orders = await _context.HoaDons
                .Where(h => h.NgayDatHang >= startDate)
                .Where(h => h.TrangThai == "Đang giao" || h.TrangThai == "Đã hoàn thành")
                .ToListAsync(); // Thực thi câu lệnh lấy dữ liệu ngay tại đây

            // BƯỚC 2: Xử lý GroupBy và Tính tổng bằng C# (Chính xác tuyệt đối)
            var result = orders
                .GroupBy(h => h.NgayDatHang.Date) // Nhóm theo ngày (phần Date)
                .OrderBy(g => g.Key)              // Sắp xếp theo ngày tăng dần
                .Select(g => new
                {
                    Date = g.Key.ToString("dd/MM/yyyy"), // Định dạng ngày Việt Nam
                    Revenue = g.Sum(h => h.TongTien)     // Tính tổng tiền
                })
                .ToList();

            return Json(result);
        }
    }
}