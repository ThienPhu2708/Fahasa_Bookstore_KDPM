using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CNPM_LIBRARY_MANAGEMENT.ViewComponents
{
    [ViewComponent(Name = "SachSubTypeMenu")]
    public class SachSubTypeMenuComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public SachSubTypeMenuComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var danhSachTheLoai = await _context.TheLoaiSaches
                                        .OrderBy(tls => tls.Id)
                                        .ToListAsync();
            return View(danhSachTheLoai);
        }
    }
}