using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CNPM_LIBRARY_MANAGEMENT.ViewComponents
{
    [ViewComponent(Name = "TypeMenu")]
    public class TypeMenuComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public TypeMenuComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var danhSachLoaiSP = await _context.LoaiSanPhams
                .OrderBy(lsp => lsp.Id)
                .ToListAsync();

            return View(danhSachLoaiSP);
        }
    }
}