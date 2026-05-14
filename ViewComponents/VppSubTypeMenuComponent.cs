using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CNPM_LIBRARY_MANAGEMENT.ViewComponents
{
    [ViewComponent(Name = "VppSubTypeMenu")]
    public class VppSubTypeMenuComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public VppSubTypeMenuComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var danhSachVPP = await _context.LoaiVpphams
                                        .OrderBy(lvpp => lvpp.Id)
                                        .ToListAsync();

            return View(danhSachVPP);
        }
    }
}