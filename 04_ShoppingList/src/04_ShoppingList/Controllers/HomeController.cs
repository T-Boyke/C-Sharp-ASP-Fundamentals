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
        Repository.AddResponse(position);
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
    /// </summary>
    /// <returns>Das View-Ergebnis mit allen Artikeln im Modell.</returns>
    [HttpGet]
    public IActionResult ArtikelAnsehen()
    {
        var positions = Repository.Positions;
        return View(positions);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

