using CNPM_LIBRARY_MANAGEMENT.Data; 
using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using CNPM_LIBRARY_MANAGEMENT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_LIBRARY_MANAGEMENT.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.SanPhams
                                .Include(s => s.ChiTietSach).ThenInclude(c => c.TacGia)
                                .Include(s => s.ChiTietVpp).ThenInclude(c => c.ThuongHieu)
                                .OrderByDescending(s => s.SoLuongDaBan)
                                .Take(8)
                                .ToListAsync();

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}