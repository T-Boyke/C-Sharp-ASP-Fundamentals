using IdentityExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IdentityExample.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }
        [Authorize]
        public IActionResult LoginOnly() {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
