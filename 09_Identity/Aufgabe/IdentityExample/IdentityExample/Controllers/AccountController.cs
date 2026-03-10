using IdentityExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityExample.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl) {
            var model = new LoginModel() {
                Password = string.Empty,
                Username = string.Empty,
                ReturnUrl = returnUrl
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model) {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null) {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(
                        user, model.Password, false, false);
                    if (result.Succeeded) {
                        return Redirect(model.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("", "Username oder Passwort ungültig");
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl = "/") {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
        public IActionResult AccessDenied(string returnUrl) {
            return View("AccessDenied", returnUrl);
        }
    }
}
