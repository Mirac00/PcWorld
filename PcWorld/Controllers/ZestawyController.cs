using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PcWorld.Data;
using PcWorld.Models;
using System.Linq;

namespace PcWorld.Controllers
{
    public class ZestawyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZestawyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["CategorySortParm"] = sortOrder == "category" ? "category_desc" : "category";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["CurrentFilter"] = searchString;

            var sets = from s in _context.Sets
                       select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                sets = sets.Where(s => s.Name.Contains(searchString)
                                       || s.Description.Contains(searchString)
                                       || s.Category.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name":
                    sets = sets.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    sets = sets.OrderByDescending(s => s.Name);
                    break;
                case "category":
                    sets = sets.OrderBy(s => s.Category.Name);
                    break;
                case "category_desc":
                    sets = sets.OrderByDescending(s => s.Category.Name);
                    break;
                case "price":
                    sets = sets.OrderBy(s => s.Keyboard.Price + s.Mouse.Price);
                    break;
                case "price_desc":
                    sets = sets.OrderByDescending(s => s.Keyboard.Price + s.Mouse.Price);
                    break;
                default:
                    sets = sets.OrderBy(s => s.Name);
                    break;
            }
            return View(sets.ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var set = _context.Sets
                .Include(s => s.Keyboard)
                .Include(s => s.Mouse)
                .FirstOrDefault(m => m.Id == id);
            if (set == null)
            {
                return NotFound();
            }

            return View(set);
        }

        public IActionResult Create()
        {
            ViewData["KeyboardId"] = new SelectList(_context.Keyboards, "Id", "Name");
            ViewData["MouseId"] = new SelectList(_context.Mice, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KeyboardId,MouseId")] Set set)
        {
            if (ModelState.IsValid)
            {
                _context.Add(set);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KeyboardId"] = new SelectList(_context.Keyboards, "Id", "Name", set.KeyboardId);
            ViewData["MouseId"] = new SelectList(_context.Mice, "Id", "Name", set.MouseId);
            return View(set);
        }

        // GET: Zestawy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound();
            }
            ViewData["KeyboardId"] = new SelectList(_context.Keyboards, "Id", "Name", set.KeyboardId);
            ViewData["MouseId"] = new SelectList(_context.Mice, "Id", "Name", set.MouseId);
            return View(set);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KeyboardId,MouseId")] Set set)
        {
            if (id != set.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(set);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SetExists(set.Id))
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
            ViewData["KeyboardId"] = new SelectList(_context.Keyboards, "Id", "Name", set.KeyboardId);
            ViewData["MouseId"] = new SelectList(_context.Mice, "Id", "Name", set.MouseId);
            return View(set);
        }

        // GET: Zestawy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var set = await _context.Sets
                .Include(s => s.Keyboard)
                .Include(s => s.Mouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (set == null)
            {
                return NotFound();
            }

            return View(set);
        }

        // POST: Zestawy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zestaw = await _context.Sets.FindAsync(id);
            _context.Sets.Remove(zestaw);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZestawExists(int id)
        {
            return _context.Sets.Any(e => e.Id == id);
        }
    }
}

