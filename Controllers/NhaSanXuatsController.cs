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
    public class NhaSanXuatsController : Controller
    {
        private readonly AppDbContext _context;

        public NhaSanXuatsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: NhaSanXuats
        public async Task<IActionResult> Index()
        {
            return View(await _context.NhaSanXuats.ToListAsync());
        }

        // GET: NhaSanXuats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _context.NhaSanXuats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhaSanXuat == null)
            {
                return NotFound();
            }

            return View(nhaSanXuat);
        }

        // GET: NhaSanXuats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaSanXuats/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenNsx")] NhaSanXuat nhaSanXuat)
        {
            ModelState.Remove("MaNsx");
            if (ModelState.IsValid)
            {
                _context.Add(nhaSanXuat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhaSanXuat);
        }

        // GET: NhaSanXuats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id);
            if (nhaSanXuat == null)
            {
                return NotFound();
            }
            return View(nhaSanXuat);
        }

        // POST: NhaSanXuats/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenNsx")] NhaSanXuat nhaSanXuat)
        {
            if (id != nhaSanXuat.Id)
            {
                return NotFound();
            }

            ModelState.Remove("MaNsx");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaSanXuat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaSanXuatExists(nhaSanXuat.Id))
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
            return View(nhaSanXuat);
        }

        // GET: NhaSanXuats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _context.NhaSanXuats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhaSanXuat == null)
            {
                return NotFound();
            }

            return View(nhaSanXuat);
        }

        // POST: NhaSanXuats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id);
            if (nhaSanXuat != null)
            {
                _context.NhaSanXuats.Remove(nhaSanXuat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhaSanXuatExists(int id)
        {
            return _context.NhaSanXuats.Any(e => e.Id == id);
        }
    }
}