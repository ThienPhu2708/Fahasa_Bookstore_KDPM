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
    public class ThuongHieusController : Controller
    {
        private readonly AppDbContext _context;

        public ThuongHieusController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ThuongHieus
        public async Task<IActionResult> Index()
        {
            return View(await _context.ThuongHieus.ToListAsync());
        }

        // GET: ThuongHieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }

        // GET: ThuongHieus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ThuongHieus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenThuongHieu")] ThuongHieu thuongHieu)
        {
            ModelState.Remove("MaThuongHieu");
            if (ModelState.IsValid)
            {
                _context.Add(thuongHieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(thuongHieu);
        }

        // GET: ThuongHieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus.FindAsync(id);
            if (thuongHieu == null)
            {
                return NotFound();
            }
            return View(thuongHieu);
        }

        // POST: ThuongHieus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenThuongHieu")] ThuongHieu thuongHieu)
        {
            if (id != thuongHieu.Id)
            {
                return NotFound();
            }

            ModelState.Remove("MaThuongHieu");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thuongHieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThuongHieuExists(thuongHieu.Id))
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
            return View(thuongHieu);
        }

        // GET: ThuongHieus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }

        // POST: ThuongHieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thuongHieu = await _context.ThuongHieus.FindAsync(id);
            if (thuongHieu != null)
            {
                _context.ThuongHieus.Remove(thuongHieu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThuongHieuExists(int id)
        {
            return _context.ThuongHieus.Any(e => e.Id == id);
        }
    }
}