using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
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
    /// Zeigt das Impressum der Anwendung an.
    /// </summary>
    /// <returns>Die Impressum-View.</returns>
    public IActionResult Impressum() => View();

    /// <summary>
    /// Zeigt die Fehlerseite der Anwendung an.
    /// Wird standardmäßig von der ExceptionHandler Middleware aufgerufen.
    /// </summary>
    /// <returns>Die Error-View.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        logger.LogError("Ein unbehandelter Fehler ist aufgetreten.");
        return View();
    }

    /// <summary>
    /// Setzt die Sprache der Anwendung.
    /// </summary>
    /// <param name="culture">Die gewählte Kultur.</param>
    /// <param name="returnUrl">Die URL, zu der zurückgekehrt werden soll.</param>
    /// <returns>Ein LocalRedirect zur Rücksprung-URL.</returns>
    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return LocalRedirect(returnUrl);
    }
}
