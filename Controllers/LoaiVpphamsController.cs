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
    // [Authorize(Roles = "Admin")]
    public class LoaiVpphamsController : Controller
    {
        private readonly AppDbContext _context;

        public LoaiVpphamsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: LoaiVpphams
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiVpphams.ToListAsync());
        }

        // GET: LoaiVpphams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiVppham = await _context.LoaiVpphams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiVppham == null)
            {
                return NotFound();
            }

            return View(loaiVppham);
        }

        // GET: LoaiVpphams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiVpphams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenLoaiVpp")] LoaiVppham loaiVppham)
        {
            ModelState.Remove("MaLoaiVpp");
            if (ModelState.IsValid)
            {
                _context.Add(loaiVppham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiVppham);
        }

        // GET: LoaiVpphams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiVppham = await _context.LoaiVpphams.FindAsync(id);
            if (loaiVppham == null)
            {
                return NotFound();
            }
            return View(loaiVppham);
        }

        // POST: LoaiVpphams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenLoaiVpp")] LoaiVppham loaiVppham)
        {
            if (id != loaiVppham.Id)
            {
                return NotFound();
            }

            ModelState.Remove("MaLoaiVpp");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiVppham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiVpphamExists(loaiVppham.Id))
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
            return View(loaiVppham);
        }

        // GET: LoaiVpphams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiVppham = await _context.LoaiVpphams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiVppham == null)
            {
                return NotFound();
            }

            return View(loaiVppham);
        }

        // POST: LoaiVpphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiVppham = await _context.LoaiVpphams.FindAsync(id);
            if (loaiVppham != null)
            {
                _context.LoaiVpphams.Remove(loaiVppham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiVpphamExists(int id)
        {
            return _context.LoaiVpphams.Any(e => e.Id == id);
        }
    }
}