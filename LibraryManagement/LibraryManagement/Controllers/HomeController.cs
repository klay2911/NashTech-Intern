using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;

namespace LibraryManagement.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult HomeForNormalUser()
    {
        return View();
    }

    public IActionResult HomeForSuperUser()
    {

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}