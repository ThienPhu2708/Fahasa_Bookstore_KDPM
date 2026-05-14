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
    public class ChatLieusController : Controller
    {
        private readonly AppDbContext _context;

        public ChatLieusController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ChatLieus
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChatLieus.ToListAsync());
        }

        // GET: ChatLieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatLieu = await _context.ChatLieus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatLieu == null)
            {
                return NotFound();
            }

            return View(chatLieu);
        }

        // GET: ChatLieus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatLieus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenChatLieu")] ChatLieu chatLieu)
        {
            ModelState.Remove("MaChatLieu");
            if (ModelState.IsValid)
            {
                _context.Add(chatLieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatLieu);
        }

        // GET: ChatLieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatLieu = await _context.ChatLieus.FindAsync(id);
            if (chatLieu == null)
            {
                return NotFound();
            }
            return View(chatLieu);
        }

        // POST: ChatLieus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenChatLieu")] ChatLieu chatLieu)
        {
            if (id != chatLieu.Id)
            {
                return NotFound();
            }

            ModelState.Remove("MaChatLieu");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatLieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatLieuExists(chatLieu.Id))
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
            return View(chatLieu);
        }

        // GET: ChatLieus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatLieu = await _context.ChatLieus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatLieu == null)
            {
                return NotFound();
            }

            return View(chatLieu);
        }

        // POST: ChatLieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatLieu = await _context.ChatLieus.FindAsync(id);
            if (chatLieu != null)
            {
                _context.ChatLieus.Remove(chatLieu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatLieuExists(int id)
        {
            return _context.ChatLieus.Any(e => e.Id == id);
        }
    }
}