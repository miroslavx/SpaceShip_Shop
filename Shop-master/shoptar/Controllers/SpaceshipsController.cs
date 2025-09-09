using Microsoft.AspNetCore.Mvc;
using shoptar.Core.Domain; 
using shoptar.Data;
using shoptar.Models.Spaceships;
using System.Threading.Tasks; 

namespace shoptar.Controllers
{
    public class SpaceshipsController : Controller
    {
        private readonly ShoptarContext _context;

        public SpaceshipsController(ShoptarContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var result = _context.Spaceships
                .Select(x => new SpaceshipsIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    BuiltDate = x.BuiltDate,
                    TypeName = x.TypeName,
                    Crew = x.Crew
                });

            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(Spaceship spaceship)
        {
            if (ModelState.IsValid)
            {
                spaceship.Id = Guid.NewGuid();
                spaceship.CreatedAt = DateTime.Now;
                spaceship.ModifiedAt = DateTime.Now;
                _context.Add(spaceship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spaceship);
        }
    }
}