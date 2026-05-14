using CNPM_LIBRARY_MANAGEMENT.Data;
using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using CNPM_LIBRARY_MANAGEMENT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_LIBRARY_MANAGEMENT.ViewComponents
{
    public class MegaMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public MegaMenuViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy danh sách Thể loại sách và Loại VPP từ DB
            var model = new MegaMenuViewModel
            {
                TheLoaiSaches = await _context.TheLoaiSaches.OrderBy(x => x.TenTheLoai).ToListAsync(),
                LoaiVpphams = await _context.LoaiVpphams.OrderBy(x => x.TenLoaiVpp).ToListAsync()
            };

            return View(model);
        }
    }
}