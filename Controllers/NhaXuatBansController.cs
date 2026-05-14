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
    public class NhaXuatBansController : Controller
    {
        private readonly AppDbContext _context;

        public NhaXuatBansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: NhaXuatBans
        public async Task<IActionResult> Index()
        {
            return View(await _context.NhaXuatBans.ToListAsync());
        }

        // GET: NhaXuatBans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaXuatBan = await _context.NhaXuatBans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhaXuatBan == null)
            {
                return NotFound();
            }

            return View(nhaXuatBan);
        }

        // GET: NhaXuatBans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaXuatBans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenNxb")] NhaXuatBan nhaXuatBan)
        {
            ModelState.Remove("MaNxb");
            if (ModelState.IsValid)
            {
                _context.Add(nhaXuatBan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhaXuatBan);
        }

        // GET: NhaXuatBans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaXuatBan = await _context.NhaXuatBans.FindAsync(id);
            if (nhaXuatBan == null)
            {
                return NotFound();
            }
            return View(nhaXuatBan);
        }

        // POST: NhaXuatBans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenNxb")] NhaXuatBan nhaXuatBan)
        {
            if (id != nhaXuatBan.Id)
            {
                return NotFound();
            }

            ModelState.Remove("MaNxb");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaXuatBan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaXuatBanExists(nhaXuatBan.Id))
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
            return View(nhaXuatBan);
        }

        // GET: NhaXuatBans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaXuatBan = await _context.NhaXuatBans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhaXuatBan == null)
            {
                return NotFound();
            }

            return View(nhaXuatBan);
        }

        // POST: NhaXuatBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhaXuatBan = await _context.NhaXuatBans.FindAsync(id);
            if (nhaXuatBan != null)
            {
                _context.NhaXuatBans.Remove(nhaXuatBan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhaXuatBanExists(int id)
        {
            return _context.NhaXuatBans.Any(e => e.Id == id);
        }
    }
}