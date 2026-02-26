using Microsoft.AspNetCore.Mvc;
using Unit_03_RSVP_App.Models;

namespace Unit_03_RSVP_App.Controllers;

/// <summary>
/// Verwaltet die Hauptfunktionen der RSVP-Anwendung, einschließlich der Startseite,
/// des RSVP-Formulars und der Gästeliste.
/// </summary>
/// <remarks>
/// Dieser Controller demonstriert das MVC-Entwurfsmuster und die Trennung von Belangen (Separation of Concerns).
/// Er verwendet In-Memory-Speicherung über das <see cref="Repository"/>.
/// </remarks>
public class HomeController : Controller
{
    /// <summary>
    /// Zeigt die Willkommensseite der Anwendung an.
    /// </summary>
    /// <returns>Ein <see cref="ViewResult"/>, das die Index-Ansicht rendert.</returns>
    /// <remarks>
    /// Ermittelt die passende Begrüßung basierend auf der aktuellen Uhrzeit.
    /// </remarks>
    public IActionResult Index()
    {
        int hour = DateTime.Now.Hour;
        ViewBag.Greeting = hour < 12 ? "Guten Morgen" : "Guten Tag";
        return View();
    }

    /// <summary>
    /// Zeigt das leere RSVP-Formular an (HTTP GET).
    /// </summary>
    /// <returns>Die Ansicht für das RSVP-Formular.</returns>
    [HttpGet]
    public ViewResult RsvpForm()
    {
        return View();
    }

    /// <summary>
    /// Verarbeitet die abgesendeten RSVP-Daten (HTTP POST).
    /// </summary>
    /// <param name="guestResponse">Das vom Model-Binder befüllte <see cref="GuestResponse"/> Objekt.</param>
    /// <returns>
    /// Die Bestätigungsseite bei gültigen Daten, andernfalls erneut das Formular mit Fehlermeldungen.
    /// </returns>
    /// <remarks>
    /// Implementiert das PRG-Pattern (Post-Redirect-Get) indirekt durch Rückgabe einer spezifischen View.
    /// </remarks>
    [HttpPost]
    public ViewResult RsvpForm(GuestResponse guestResponse)
    {
        if (ModelState.IsValid)
        {
            Repository.AddResponse(guestResponse);
            return View("Thanks", guestResponse);
        }
        else
        {
            // Validerungsfehler vorhanden
            return View();
        }
    }

    /// <summary>
    /// Zeigt eine Liste aller Gäste an, die zugesagt haben.
    /// </summary>
    /// <returns>Die Ansicht mit der Liste der teilnehmenden Gäste.</returns>
    public ViewResult ListResponses()
    {
        return View(Repository.Responses.Where(r => r.WillAttend == true));
    }
}
