using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNPM_LIBRARY_MANAGEMENT.Data.Models;

namespace CNPM_LIBRARY_MANAGEMENT.Controllers
{
    // Chúng ta sẽ dùng layout admin cho tất cả
    // [Authorize(Roles = "Admin")]
    public class TheLoaiSachesController : Controller
    {
        private readonly AppDbContext _context;

        public TheLoaiSachesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TheLoaiSaches
        public async Task<IActionResult> Index()
        {
            return View(await _context.TheLoaiSaches.ToListAsync());
        }

        // GET: TheLoaiSaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theLoaiSach = await _context.TheLoaiSaches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (theLoaiSach == null)
            {
                return NotFound();
            }

            return View(theLoaiSach);
        }

        // GET: TheLoaiSaches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TheLoaiSaches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenTheLoai")] TheLoaiSach theLoaiSach)
        {
            // Bỏ qua "MaTheLoai" vì nó được tạo tự động
            ModelState.Remove("MaTheLoai");
            if (ModelState.IsValid)
            {
                _context.Add(theLoaiSach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theLoaiSach);
        }

        // GET: TheLoaiSaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theLoaiSach = await _context.TheLoaiSaches.FindAsync(id);
            if (theLoaiSach == null)
            {
                return NotFound();
            }
            return View(theLoaiSach);
        }

        // POST: TheLoaiSaches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenTheLoai")] TheLoaiSach theLoaiSach)
        {
            if (id != theLoaiSach.Id)
            {
                return NotFound();
            }

            ModelState.Remove("MaTheLoai");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theLoaiSach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheLoaiSachExists(theLoaiSach.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(theLoaiSach);
        }

        // GET: TheLoaiSaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theLoaiSach = await _context.TheLoaiSaches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (theLoaiSach == null)
            {
                return NotFound();
            }

            return View(theLoaiSach);
        }

        // POST: TheLoaiSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var theLoaiSach = await _context.TheLoaiSaches.FindAsync(id);
            if (theLoaiSach != null)
            {
                _context.TheLoaiSaches.Remove(theLoaiSach);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TheLoaiSachExists(int id)
        {
            return _context.TheLoaiSaches.Any(e => e.Id == id);
        }
    }
}