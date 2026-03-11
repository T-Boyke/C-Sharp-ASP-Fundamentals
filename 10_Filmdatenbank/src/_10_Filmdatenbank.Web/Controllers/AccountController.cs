using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Verwaltet Benutzerkonten, Anmeldungen und Abmeldungen.
/// </summary>
/// <param name="signInManager">Manager für den Anmeldeprozess.</param>
public class AccountController(SignInManager<IdentityUser> signInManager) : Controller
{
    /// <summary>
    /// Zeigt die Anmeldeseite an.
    /// </summary>
    /// <returns>Die Login-View.</returns>
    public IActionResult Login() => View();

    /// <summary>
    /// Verarbeitet den Anmeldeversuch eines Benutzers.
    /// </summary>
    /// <param name="model">Das ViewModel mit den Login-Daten.</param>
    /// <returns>Redirect zur Startseite bei Erfolg, andernfalls die Login-View mit Fehlermeldung.</returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded) return RedirectToAction("Index", "Home");
            ViewBag.LoginFailed = true;
            ModelState.AddModelError("", "Ungültiger Login-Versuch.");
        }
        return View(model);
    }

    /// <summary>
    /// Meldet den aktuellen Benutzer ab.
    /// </summary>
    /// <returns>Redirect zur Startseite.</returns>
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Zeigt die Seite für verweigerten Zugriff an.
    /// </summary>
    /// <returns>Die AccessDenied-View.</returns>
    public IActionResult AccessDenied() => View();
}

/// <summary>
/// ViewModel für den Login-Prozess.
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// Die E-Mail-Adresse des Benutzers.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Das Passwort des Benutzers.
    /// </summary>
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
