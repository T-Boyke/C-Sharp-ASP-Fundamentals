using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Controller für die Startseite und allgemeine Seiten.
/// </summary>
/// <param name="logger">Der Logger-Dienst für diesen Controller.</param>
public class HomeController(ILogger<HomeController> logger) : Controller
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

    /// <summary>
    /// Zeigt die Fehlerseite der Anwendung an.
    /// Wird standardmäßig von der ExceptionHandler Middleware aufgerufen.
    /// </summary>
    /// <returns>Die Error-View.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        logger.LogError("Ein unbehandelter Fehler ist aufgetreten.");
        // View("Error") oder ähnliches erfordert ein ErrorViewModel
        return View();
    }
}
