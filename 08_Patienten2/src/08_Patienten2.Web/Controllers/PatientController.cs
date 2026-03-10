using _08_Patienten2.Application.Interfaces;
using _08_Patienten2.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace _08_Patienten2.Web.Controllers;

/// <summary>
/// Controller für die Verwaltung von Patienten (RBAC integriert).
/// </summary>
[Authorize]
public class PatientController(IPatientService service) : Controller
{
    /// <summary>
    /// Zeigt die Liste aller Patienten an.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var patients = await service.GetAllPatientsAsync();
        return View(patients);
    }

    /// <summary>
    /// Zeigt das Formular zum Anlegen eines neuen Patienten (nur Admin).
    /// </summary>
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    /// <summary>
    /// Verarbeitet das Anlegen eines neuen Patienten.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PatientCreateDto createDto)
    {
        if (!ModelState.IsValid) return View(createDto);
        
        await service.CreatePatientAsync(createDto);
        return RedirectToAction(nameof(Index));
    }
}
