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
    public class MauSacsController : Controller
    {
        private readonly AppDbContext _context;

        public MauSacsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MauSacs
        public async Task<IActionResult> Index()
        {
            return View(await _context.MauSacs.ToListAsync());
        }

        // GET: MauSacs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mauSac = await _context.MauSacs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mauSac == null)
            {
                return NotFound();
            }

            return View(mauSac);
        }

        // GET: MauSacs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MauSacs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenMauSac")] MauSac mauSac)
        {
            ModelState.Remove("MaMauSac");
            if (ModelState.IsValid)
            {
                _context.Add(mauSac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mauSac);
        }

        // GET: MauSacs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mauSac = await _context.MauSacs.FindAsync(id);
            if (mauSac == null)
            {
                return NotFound();
            }
            return View(mauSac);
        }

        // POST: MauSacs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenMauSac")] MauSac mauSac)
        {
            if (id != mauSac.Id)
            {
                return NotFound();
            }

            ModelState.Remove("MaMauSac");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mauSac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MauSacExists(mauSac.Id))
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
            return View(mauSac);
        }

        // GET: MauSacs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mauSac = await _context.MauSacs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mauSac == null)
            {
                return NotFound();
            }

            return View(mauSac);
        }

        // POST: MauSacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mauSac = await _context.MauSacs.FindAsync(id);
            if (mauSac != null)
            {
                _context.MauSacs.Remove(mauSac);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MauSacExists(int id)
        {
            return _context.MauSacs.Any(e => e.Id == id);
        }
    }
}