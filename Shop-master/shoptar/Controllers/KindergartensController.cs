using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoptar.Core.Domain;
using shoptar.Data;
using shoptar.Models.Kindergartens;
using System.Threading.Tasks;

namespace shoptar.Controllers
{
    public class KindergartensController : Controller
    {
        private readonly ShoptarContext _context;

        public KindergartensController(ShoptarContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Kindergartens
                .Select(x => new KindergartensIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToListAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            return View(kindergarten);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kindergarten kindergarten)
        {
            if (ModelState.IsValid)
            {
                kindergarten.Id = Guid.NewGuid();
                kindergarten.CreatedAt = DateTime.Now;
                kindergarten.UpdatedAt = DateTime.Now;

                _context.Add(kindergarten);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kindergarten);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindergarten = await _context.Kindergartens.FindAsync(id);
            if (kindergarten == null)
            {
                return NotFound();
            }
            return View(kindergarten);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Kindergarten kindergarten)
        {
            if (id != kindergarten.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entityToUpdate = await _context.Kindergartens.FindAsync(id);
                    if (entityToUpdate == null)
                    {
                        return NotFound();
                    }

                    entityToUpdate.GroupName = kindergarten.GroupName;
                    entityToUpdate.ChildrenCount = kindergarten.ChildrenCount;
                    entityToUpdate.KindergartenName = kindergarten.KindergartenName;
                    entityToUpdate.TeacherName = kindergarten.TeacherName;
                    entityToUpdate.UpdatedAt = DateTime.Now;

                    _context.Update(entityToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Kindergartens.Any(e => e.Id == kindergarten.Id))
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
            return View(kindergarten);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            return View(kindergarten);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var kindergarten = await _context.Kindergartens.FindAsync(id);
            if (kindergarten != null)
            {
                _context.Kindergartens.Remove(kindergarten);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}