using Microsoft.AspNetCore.Mvc;
using _07_Patienten.Domain.Entities;
using _07_Patienten.Domain.Interfaces;

namespace _07_Patienten.Controllers;

/// <summary>
/// Controller für Patientenuntersuchungen.
/// </summary>
public class ExaminationsController : Controller
{
    private readonly IExaminationRepository _examinationRepo;

    public ExaminationsController(IExaminationRepository examinationRepo)
    {
        _examinationRepo = examinationRepo;
    }

    // GET: Examinations/Create?patientId=5
    public IActionResult Create(int patientId)
    {
        ViewBag.PatientId = patientId;
        return View();
    }

    // POST: Examinations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Examination examination)
    {
        if (ModelState.IsValid)
        {
            await _examinationRepo.AddAsync(examination);
            return RedirectToAction("Details", "Patients", new { id = examination.PatientId });
        }
        ViewBag.PatientId = examination.PatientId;
        return View(examination);
    }

    // POST: Examinations/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int patientId)
    {
        await _examinationRepo.DeleteAsync(id);
        return RedirectToAction("Details", "Patients", new { id = patientId });
    }
}
