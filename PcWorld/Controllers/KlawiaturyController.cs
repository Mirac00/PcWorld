using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcWorld.Data;
using PcWorld.Models;
using System.Linq;

namespace PcWorld.Controllers
{
    public class KlawiaturyController : Controller
    {
        private readonly ApplicationDbContext _context;
        public KlawiaturyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Klawiatury
        public IActionResult Index(string sortOrder, string currentFilter, string search)
        {
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.RatingSortParm = sortOrder == "Rating" ? "rating_desc" : "Rating";

            if (search != null)
            {
                currentFilter = search;
            }

            ViewBag.CurrentFilter = search;

            var keyboards = from k in _context.Keyboards select k;

            switch (sortOrder)
            {
                case "Name":
                    keyboards = keyboards.OrderBy(k => k.Name);
                    break;
                case "name_desc":
                    keyboards = keyboards.OrderByDescending(k => k.Name);
                    break;
                case "Category":
                    keyboards = keyboards.OrderBy(k => k.Category.Name);
                    break;
                case "category_desc":
                    keyboards = keyboards.OrderByDescending(k => k.Category.Name);
                    break;
                case "Price":
                    keyboards = keyboards.OrderBy(k => k.Price);
                    break;
                case "price_desc":
                    keyboards = keyboards.OrderByDescending(k => k.Price);
                    break;
                case "Rating":
                    keyboards = keyboards.OrderBy(k => k.Reviews.Average(r => r.Rating));
                    break;
                case "rating_desc":
                    keyboards = keyboards.OrderByDescending(k => k.Reviews.Average(r => r.Rating));
                    break;
                default:
                    keyboards = keyboards.OrderBy(k => k.Name);
                    break;
            }

            if (!string.IsNullOrEmpty(search))
            {
                keyboards = keyboards.Where(k => k.Name.Contains(search) ||
                                                k.Category.Name.Contains(search) ||
                                                k.Description.Contains(search));
            }

            return View(keyboards);
        }

        // GET: Klawiatury/Details/5
        public IActionResult Details(int id)
        {
            var keyboard = _context.Keyboards.FirstOrDefault(k => k.Id == id);
            if (keyboard == null)
            {
                return NotFound();
            }
            return View(keyboard);
        }
        // GET: Klawiatury/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CategoryId,Price")] Keyboard keyboard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(keyboard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(keyboard);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var keyboard = await _context.Keyboards.FindAsync(id);
            if (keyboard == null)
            {
                return NotFound();
            }
            return View(keyboard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CategoryId,Price")] Keyboard keyboard)
        {
            if (id != keyboard.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keyboard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyboardExists(keyboard.Id))
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
            return View(keyboard);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var keyboard = await _context.Keyboards
    .FirstOrDefaultAsync(m => m.Id == id);
            if (keyboard == null)
            {
                return NotFound();
            }

            return View(keyboard);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var keyboard = await _context.Keyboards.FindAsync(id);
            _context.Keyboards.Remove(keyboard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeyboardExists(int id)
        {
            return _context.Keyboards.Any(e => e.Id == id);
        }
    }
}



