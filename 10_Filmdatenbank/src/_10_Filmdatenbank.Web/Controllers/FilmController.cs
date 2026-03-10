using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _10_Filmdatenbank.Web.Controllers
{
    [Authorize]
    public class FilmController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var filme = await _context.Filme
                .Include(f => f.PersonEigenschaftFilme)
                    .ThenInclude(pef => pef.Person)
                .Include(f => f.PersonEigenschaftFilme)
                    .ThenInclude(pef => pef.Eigenschaft)
                .OrderBy(f => f.Titel)
                .ToListAsync();
            return View(filme);
        }

        public async Task<IActionResult> Details(int id)
        {
            var film = await _context.Filme
                .Include(f => f.PersonEigenschaftFilme)
                    .ThenInclude(pef => pef.Person)
                .Include(f => f.PersonEigenschaftFilme)
                    .ThenInclude(pef => pef.Eigenschaft)
                .FirstOrDefaultAsync(f => f.FilmID == id);

            if (film == null) return NotFound();
            return View(film);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Filme.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var film = await _context.Filme.FindAsync(id);
            if (film == null) return NotFound();
            return View(film);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Update(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var film = await _context.Filme.FindAsync(id);
            if (film == null) return NotFound();
            return View(film);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Filme.FindAsync(id);
            if (film != null)
            {
                _context.Filme.Remove(film);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
