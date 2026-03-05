using Microsoft.AspNetCore.Mvc;
using _06_NewsApplication2.Domain.Entities;
using _06_NewsApplication2.Domain.Interfaces;

namespace _06_NewsApplication2.Controllers;

/// <summary>
/// Asynchroner Controller für die Verwaltung von Autoren.
/// </summary>
public class AuthorsController : Controller
{
    private readonly IAuthorRepository _authorRepo;

    public AuthorsController(IAuthorRepository authorRepo)
    {
        _authorRepo = authorRepo;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _authorRepo.GetAllAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Firstname,Lastname")] Author author)
    {
        if (ModelState.IsValid)
        {
            await _authorRepo.AddAsync(author);
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var author = await _authorRepo.GetByIdAsync(id);
        if (author == null) return NotFound();
        return View(author);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Firstname,Lastname")] Author author)
    {
        if (id != author.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _authorRepo.UpdateAsync(author);
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var author = await _authorRepo.GetByIdAsync(id);
        if (author == null) return NotFound();
        return View(author);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _authorRepo.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
