using _09_Identity.Application.DTOs;
using _09_Identity.Domain.Interfaces;
using _09_Identity.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _09_Identity.Web.Controllers;

/// <summary>
/// Controller für Identity-Operationen (Login, Logout, Access Denied).
/// </summary>
public class AccountController(IAuthService authService) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginDto(string.Empty, string.Empty, returnUrl));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var credentials = new Credentials(model.Username, model.Password);
            var success = await authService.LoginAsync(credentials);

            if (success)
            {
                return LocalRedirect(model.ReturnUrl ?? "/");
            }
            
            ModelState.AddModelError(string.Empty, "Ungültiger Benutzername oder Passwort.");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
