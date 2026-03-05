using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using _06_NewsApplication2.Domain.Entities;
using _06_NewsApplication2.Domain.Interfaces;

namespace _06_NewsApplication2.Controllers;

/// <summary>
/// Asynchroner Controller für die Verwaltung von News-Artikeln.
/// </summary>
public class ArticlesController : Controller
{
    private readonly IArticleRepository _articleRepo;
    private readonly IAuthorRepository _authorRepo;

    public ArticlesController(IArticleRepository articleRepo, IAuthorRepository authorRepo)
    {
        _articleRepo = articleRepo;
        _authorRepo = authorRepo;
    }

    // GET: Articles
    public async Task<IActionResult> Index()
    {
        var articles = await _articleRepo.GetAllAsync();
        return View(articles);
    }

    // GET: Articles/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var article = await _articleRepo.GetByIdAsync(id);
        if (article == null) return NotFound();
        return View(article);
    }

    // GET: Articles/Create
    public async Task<IActionResult> Create()
    {
        await LoadAuthorsToViewDataAsync();
        return View();
    }

    // POST: Articles/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Headline,Content,AuthorId")] Article article)
    {
        if (ModelState.IsValid)
        {
            await _articleRepo.AddAsync(article);
            return RedirectToAction(nameof(Index));
        }
        await LoadAuthorsToViewDataAsync();
        return View(article);
    }

    // GET: Articles/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _articleRepo.GetByIdAsync(id);
        if (article == null) return NotFound();
        await LoadAuthorsToViewDataAsync();
        return View(article);
    }

    // POST: Articles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Headline,Content,AuthorId,CreatedAt")] Article article)
    {
        if (id != article.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _articleRepo.UpdateAsync(article);
            return RedirectToAction(nameof(Index));
        }
        await LoadAuthorsToViewDataAsync();
        return View(article);
    }

    // GET: Articles/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _articleRepo.GetByIdAsync(id);
        if (article == null) return NotFound();
        return View(article);
    }

    // POST: Articles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _articleRepo.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadAuthorsToViewDataAsync()
    {
        var authors = await _authorRepo.GetAllAsync();
        ViewData["AuthorId"] = new SelectList(authors, nameof(Author.Id), nameof(Author.Fullname));
    }
}
