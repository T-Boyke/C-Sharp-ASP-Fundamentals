using Microsoft.AspNetCore.Mvc;
using _07_Patienten.Domain.Entities;
using _07_Patienten.Domain.Interfaces;

namespace _07_Patienten.Controllers;

/// <summary>
/// Controller für die Patientenverwaltung (Async CRUD).
/// </summary>
public class PatientsController : Controller
{
    private readonly IPatientRepository _patientRepo;

    public PatientsController(IPatientRepository patientRepo)
    {
        _patientRepo = patientRepo;
    }

    // GET: Patients
    public async Task<IActionResult> Index()
    {
        var patients = await _patientRepo.GetAllAsync();
        return View(patients);
    }

    // GET: Patients/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var patient = await _patientRepo.GetByIdWithExaminationsAsync(id);
        if (patient == null) return NotFound();
        return View(patient);
    }

    // GET: Patients/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Patients/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Patient patient)
    {
        if (ModelState.IsValid)
        {
            await _patientRepo.AddAsync(patient);
            return RedirectToAction(nameof(Index));
        }
        return View(patient);
    }

    // GET: Patients/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var patient = await _patientRepo.GetByIdAsync(id);
        if (patient == null) return NotFound();
        return View(patient);
    }

    // POST: Patients/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Patient patient)
    {
        if (id != patient.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _patientRepo.UpdateAsync(patient);
            return RedirectToAction(nameof(Index));
        }
        return View(patient);
    }

    // GET: Patients/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _patientRepo.GetByIdAsync(id);
        if (patient == null) return NotFound();
        return View(patient);
    }

    // POST: Patients/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _patientRepo.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
