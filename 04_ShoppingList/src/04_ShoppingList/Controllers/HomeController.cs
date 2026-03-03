using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _04_ShoppingList.Models;

namespace _04_ShoppingList.Controllers;

/// <summary>
/// Hauptcontroller für die Einkaufsliste-Anwendung.
/// Steuert die Anzeige der Startseite, das Hinzufügen von Artikeln sowie die Listenansicht.
/// </summary>
public class HomeController : Controller
{
    private readonly IShoppingListRepository _repository;

    public HomeController(IShoppingListRepository repository)
    {
        _repository = repository;
    }
    /// <summary>
    /// Zeigt die Startseite (Index.cshtml) an.
    /// </summary>
    /// <returns>Das View-Ergebnis für die Startseite.</returns>
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Liefert die Seite zur Eingabe eines neuen Artikels (ArtikelForm.cshtml) zurück.
    /// </summary>
    /// <returns>Das View-Ergebnis für das Formular.</returns>
    [HttpGet]
    public IActionResult ArtikelForm()
    {
        return View();
    }

    /// <summary>
    /// Nimmt die Formulardaten entgegen, speichert die Position im Repository und leitet zur Bestätigungsseite weiter.
    /// </summary>
    /// <param name="position">Die aus dem Formular gebundene Position.</param>
    /// <returns>Das View-Ergebnis für die Bestätigung (Angelegt.cshtml).</returns>
    [HttpPost]
    public IActionResult ArtikelForm(Position position)
    {
        _repository.AddResponse(position);
        return View("Angelegt");
    }

    /// <summary>
    /// Zeigt die Bestätigungsseite nach erfolgreichem Anlegen an.
    /// (Diese Methode ist optional direkt aufrufbar, wird hier aber für Vollständigkeit angeboten).
    /// </summary>
    /// <returns>Das View-Ergebnis für Angelegt.</returns>
    [HttpGet]
    public IActionResult Angelegt()
    {
        return View();
    }

    /// <summary>
    /// Zeigt die Liste aller bisher gespeicherten Artikel (ArtikelAnsehen.cshtml) an.
    /// Unterstützt das Filtern über einen Suchbegriff.
    /// </summary>
    /// <param name="searchString">Der optionale Suchbegriff, um die Liste zu filtern.</param>
    /// <returns>Das View-Ergebnis mit den gefilterten Artikeln im Modell.</returns>
    [HttpGet]
    public IActionResult ArtikelAnsehen(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        
        var positions = _repository.Positions;
        
        if (!string.IsNullOrEmpty(searchString))
        {
            positions = positions.Where(p => 
                p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) || 
                p.Geschaeft.Contains(searchString, StringComparison.OrdinalIgnoreCase));
        }
        
        return View(positions);
    }

    /// <summary>
    /// Löscht einen Artikel anhand der Id und leitet danach zur Listenansicht zurück.
    /// </summary>
    /// <param name="id">Die zu behandelnde Guid.</param>
    /// <returns>Eine Weiterleitung auf ArtikelAnsehen.</returns>
    [HttpPost]
    public IActionResult Loeschen(Guid id)
    {
        _repository.Delete(id);
        return RedirectToAction("ArtikelAnsehen");
    }

    /// <summary>
    /// Zeigt das Formular zum Bearbeiten eines existierenden Artikels.
    /// </summary>
    /// <param name="id">Die Id der zu bearbeitenden Position.</param>
    [HttpGet]
    public IActionResult ArtikelBearbeiten(Guid id)
    {
        var position = _repository.GetById(id);
        if (position == null)
        {
            return RedirectToAction("ArtikelAnsehen");
        }
        return View(position);
    }

    /// <summary>
    /// Speichert die Änderungen eines bearbeiteten Artikels.
    /// </summary>
    /// <param name="position">Die geänderte Position.</param>
    [HttpPost]
    public IActionResult ArtikelBearbeiten(Position position)
    {
        _repository.Update(position);
        return RedirectToAction("ArtikelAnsehen");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

