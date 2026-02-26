using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Unit_03_RSVP_App.Models;

namespace Unit_03_RSVP_App.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        int hour = DateTime.Now.Hour;
        ViewBag.Greeting = hour < 12 ? "Good Morning" : "Good Afternoon";
        return View();
    }

    [HttpGet]
    public ViewResult RsvpForm()
    {
        return View();
    }

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
            // Validation error, redisplay the form
            return View();
        }
    }

    public ViewResult ListResponses()
    {
        return View(Repository.Responses.Where(r => r.WillAttend == true));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
