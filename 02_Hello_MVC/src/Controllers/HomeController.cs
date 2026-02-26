using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Unit_02_Hello_MVC.Models;

namespace Unit_02_Hello_MVC.Controllers;

/// <summary>
/// The Controller is the 'C' in MVC. It handles user requests,
/// interacts with Models, and selects Views to render.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Default action for the Home page.
    /// URL: /Home/Index or /
    /// </summary>
    public IActionResult Index()
    {
        // Simply returns the default view (Index.cshtml)
        return View();
    }

    /// <summary>
    /// Demonstrates passing a Model to a View.
    /// URL: /Home/About
    /// </summary>
    public IActionResult About()
    {
        // 1. Create the Model (Data)
        var model = new WelcomeViewModel
        {
            Message = "Welcome to the MVC Deep Dive!",
            GeneratedAt = DateTime.Now,
            IsPriority = true
        };

        // 2. Pass the Model to the View
        // The View engine will use this model to render dynamic content.
        return View(model);
    }

    /// <summary>
    /// Privacy policy page.
    /// URL: /Home/Privacy
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Standard error handling action.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
