using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bar.Controllers;

public class SesionController : Controller
{
    [Authorize(Roles = "admin")]
    public IActionResult Admin()
    {
        return View();
    }

    [Authorize(Roles = "resto")]
    public IActionResult Resto()
    {
        return View();
    }

    [Authorize(Roles = "user")]
    public IActionResult Usser()
    {
        return View();
    }
}