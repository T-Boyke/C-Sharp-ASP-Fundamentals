using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Controller für die Verwaltung von Produktionsfirmen (Studios).
/// </summary>
[Authorize]
[Route("Studios")]
[Route("Studios/[action]")]
public class ProductionCompanyController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? searchString)
    {
        var query = context.ProductionCompanies.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(c => c.Name.Contains(searchString) || (c.Description != null && c.Description.Contains(searchString)));
            ViewData["CurrentFilter"] = searchString;
        }

        var companies = await query
            .OrderBy(c => c.Name)
            .ToListAsync();
        return View(companies);
    }

    public async Task<IActionResult> Details(int id)
    {
        var company = await context.ProductionCompanies
            .Include(c => c.Films)
                .ThenInclude(f => f.Genres)
            .Include(c => c.ProductionCompanyAwards)
            .FirstOrDefaultAsync(c => c.ProductionCompanyID == id);

        if (company == null) return NotFound();
        return View(company);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(ProductionCompany company)
    {
        if (ModelState.IsValid)
        {
            context.ProductionCompanies.Add(company);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(company);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var company = await context.ProductionCompanies.FindAsync(id);
        return company == null ? NotFound() : View(company);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(ProductionCompany company)
    {
        if (ModelState.IsValid)
        {
            context.Update(company);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(company);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var company = await context.ProductionCompanies.FindAsync(id);
        return company == null ? NotFound() : View(company);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var company = await context.ProductionCompanies.FindAsync(id);
        if (company != null)
        {
            context.ProductionCompanies.Remove(company);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
