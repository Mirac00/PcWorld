using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcWorld.Data;
using PcWorld.Models;
using System.Linq;

namespace PcWorld.Controllers
{
    public class MyszkiController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MyszkiController(ApplicationDbContext context)
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

            var mice = from m in _context.Mice select m;

            switch (sortOrder)
            {
                case "Name":
                    mice = mice.OrderBy(m => m.Name);
                    break;
                case "name_desc":
                    mice = mice.OrderByDescending(m => m.Name);
                    break;
                case "Category":
                    mice = mice.OrderBy(m => m.Category.Name);
                    break;
                case "category_desc":
                    mice = mice.OrderByDescending(m => m.Category.Name);
                    break;
                case "Price":
                    mice = mice.OrderBy(m => m.Price);
                    break;
                case "price_desc":
                    mice = mice.OrderByDescending(m => m.Price);
                    break;
                case "Rating":
                    mice = mice.OrderBy(m => m.Reviews.Average(r => r.Rating));
                    break;
                case "rating_desc":
                    mice = mice.OrderByDescending(m => m.Reviews.Average(r => r.Rating));
                    break;
                default:
                    mice = mice.OrderBy(m => m.Name);
                    break;
            }

            if (!string.IsNullOrEmpty(search))
            {
                mice = mice.Where(m => m.Name.Contains(search) ||
                                                m.Category.Name.Contains(search) ||
                                                m.Description.Contains(search));
            }

            return View(mice);
        }

        // GET: Klawiatury/Details/5
        public IActionResult Details(int id)
        {
            var mouse = _context.Mice.FirstOrDefault(m => m.Id == id);
            if (mouse == null)
            {
                return NotFound();
            }
            return View(mouse);
        }
        // GET: Klawiatury/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CategoryId,Price")] Mouse mouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(mouse);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mouse = await _context.Mice.FindAsync(id);
            if (mouse == null)
            {
                return NotFound();
            }
            return View(mouse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CategoryId,Price")] Mouse mouse)
        {
            if (id != mouse.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyboardExists(mouse.Id))
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
            return View(mouse);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mouse = await _context.Mice
    .FirstOrDefaultAsync(m => m.Id == id);
            if (mouse == null)
            {
                return NotFound();
            }

            return View(mouse);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mouse = await _context.Mice.FindAsync(id);
            _context.Mice.Remove(mouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeyboardExists(int id)
        {
            return _context.Mice.Any(e => e.Id == id);
        }
    }
}



