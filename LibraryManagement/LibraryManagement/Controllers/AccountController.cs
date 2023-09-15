using System.Security.Claims;
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers;


public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(UserLogin userLogin)
    {
        var existingUser = _userService.GetUserByUserNameAndPassword(userLogin.UserName, userLogin.Password);

        if (existingUser != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userLogin.UserName),
                new Claim(ClaimTypes.Role, existingUser.Role)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = userLogin.RememberMe });

            if (existingUser.Role == "SuperUser")
            {
                return RedirectToAction("HomeForSuperUser", "Home");
            }
            else if (existingUser.Role == "NormalUser")
            {
                return RedirectToAction("HomeForNormalUser", "Home");
            }
        }
        else
        {
            ModelState.AddModelError("", "Invalid username or password");
        }

        return View(userLogin);
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
