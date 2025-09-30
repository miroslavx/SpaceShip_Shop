using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var result = await _context.Spaceships
                .Select(x => new SpaceshipsIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    BuiltDate = x.BuiltDate,
                    TypeName = x.TypeName,
                    Crew = x.Crew
                }).ToListAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spaceship = await _context.Spaceships
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spaceship == null)
            {
                return NotFound();
            }

            return View(spaceship);
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

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spaceship = await _context.Spaceships.FindAsync(id);
            if (spaceship == null)
            {
                return NotFound();
            }
            return View(spaceship);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Spaceship spaceship)
        {
            if (id != spaceship.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entityToUpdate = await _context.Spaceships.FindAsync(id);
                    if (entityToUpdate == null)
                    {
                        return NotFound();
                    }

                    entityToUpdate.Name = spaceship.Name;
                    entityToUpdate.TypeName = spaceship.TypeName;
                    entityToUpdate.BuiltDate = spaceship.BuiltDate;
                    entityToUpdate.Crew = spaceship.Crew;
                    entityToUpdate.EnginePower = spaceship.EnginePower;
                    entityToUpdate.Passengers = spaceship.Passengers;
                    entityToUpdate.InnerVolume = spaceship.InnerVolume;
                    entityToUpdate.ModifiedAt = DateTime.Now;

                    _context.Update(entityToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Spaceships.Any(e => e.Id == spaceship.Id))
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
            return View(spaceship);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spaceship = await _context.Spaceships
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spaceship == null)
            {
                return NotFound();
            }

            return View(spaceship);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var spaceship = await _context.Spaceships.FindAsync(id);
            if (spaceship != null)
            {
                _context.Spaceships.Remove(spaceship);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}