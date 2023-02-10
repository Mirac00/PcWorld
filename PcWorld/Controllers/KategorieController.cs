using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PcWorld.Data;
using PcWorld.Models;

namespace PcWorld.Controllers
{
    public class KategorieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KategorieController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }
}
