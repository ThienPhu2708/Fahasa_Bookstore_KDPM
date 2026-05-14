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
    public class TacGiasController : Controller
    {
        private readonly AppDbContext _context;

        public TacGiasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TacGias
        public async Task<IActionResult> Index()
        {
            return View(await _context.TacGia.ToListAsync()); 
        }

        // GET: TacGias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }

            var tacGia = await _context.TacGia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tacGia == null) { return NotFound(); }

            return View(tacGia);
        }

        // GET: TacGias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TacGias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // === SỬA Ở ĐÂY ===
        public async Task<IActionResult> Create([Bind("Id,TenTacGia")] TacGium tacGia)
        {
            ModelState.Remove("MaTacGia");
            if (ModelState.IsValid)
            {
                _context.Add(tacGia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tacGia);
        }

        // GET: TacGias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) { return NotFound(); }

            var tacGia = await _context.TacGia.FindAsync(id);
            if (tacGia == null) { return NotFound(); }

            return View(tacGia);
        }

        // POST: TacGias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenTacGia")] TacGium tacGia)
        {
            if (id != tacGia.Id) { return NotFound(); }

            ModelState.Remove("MaTacGia");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tacGia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TacGiaExists(tacGia.Id)) { return NotFound(); }
                    else { throw; }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tacGia);
        }

        // GET: TacGias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }

            var tacGia = await _context.TacGia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tacGia == null) { return NotFound(); }

            return View(tacGia);
        }

        // POST: TacGias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tacGia = await _context.TacGia.FindAsync(id);
            if (tacGia != null)
            {
                _context.TacGia.Remove(tacGia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TacGiaExists(int id)
        {
            return _context.TacGia.Any(e => e.Id == id);
        }
    }
}