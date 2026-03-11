using Microsoft.AspNetCore.Mvc;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Controller für die Startseite und allgemeine Seiten.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Zeigt die Startseite der Anwendung an.
    /// </summary>
    /// <returns>Die Index-View.</returns>
    public IActionResult Index() => View();

    /// <summary>
    /// Zeigt die Datenschutzseite an.
    /// </summary>
    /// <returns>Die Privacy-View.</returns>
    public IActionResult Privacy() => View();
}
