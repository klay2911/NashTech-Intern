using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLogin userLogin)
    {
        if (!ModelState.IsValid)
        {
            return View(userLogin);
        }

        var hashedPassword = HashPassword(userLogin.Password);
        var existingUser = _userService.GetUserByUserNameAndPassword(userLogin.UserName, hashedPassword);

        if (existingUser != null)
        {
            var token = GenerateJwtToken(existingUser);
        
            var claimsPrincipal = ValidateToken(token);
            if (claimsPrincipal == null)
            {
                ModelState.AddModelError("", "Invalid token, please try again");
                return View(userLogin);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userLogin.UserName),
                new Claim(ClaimTypes.Role, existingUser.Role),
                new Claim("Token", token),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            HttpContext.Session.SetString("UserId", existingUser.UserId.ToString());

            Response.Cookies.Append("JwtToken", token, new CookieOptions { HttpOnly = true });
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = userLogin.RememberMe });

            switch (existingUser.Role)
            {
                case "SuperUser":
                    return RedirectToAction("ViewBorrowingRequests", "BorrowingRequest");
                case "NormalUser":
                    return RedirectToAction("Index", "BorrowingRequest");
            }
        }
        else
        {
            ModelState.AddModelError("", "Invalid username or password, please try again");
        }

        return View(userLogin);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Remove("BookIdsInRequest");
        Response.Cookies.Delete("JwtToken");
        return RedirectToAction("Login", "Account");
    }
    
    private string HashPassword(string password)
    { 
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }


    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("fc746b61cde4f6665d3f9791446cd5395661860c0075a905ed9810b7391af467");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] 
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("fc746b61cde4f6665d3f9791446cd5395661860c0075a905ed9810b7391af467"); 
        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
    
            return claimsPrincipal;
        }
        catch
        {
            return null;
        }
    }
    // public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string searchTerm = "")
    // {
    //     var users = await _userService.GetAllUsersAsync(pageNumber, pageSize, searchTerm);
    //     return View(users);
    // }

    // [HttpPost]
    // public async Task<IActionResult> Create(User user)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         await _userService.CreateUser(user);
    //         return RedirectToAction(nameof(Index));
    //     }
    //     return View(user);
    // }
    [Authorize(Roles = "NormalUser")]
    [HttpGet]
    public IActionResult ChangeInformation()
    {
        var userId = int.Parse(HttpContext.Session.GetString("UserId"));
        var user = _userService.GetUserById(userId);
        return View(user);
    }
    [Authorize(Roles = "NormalUser")]
    [HttpPost]
    public async Task<IActionResult> ChangeInformation(User user)
    {
        await _userService.UpdateUser(user);
            return RedirectToAction("Index","BorrowingRequest");
    }

    // [HttpGet]
    // public Task<IActionResult> Delete(int id)
    // {
    //     var user = _userService.GetUserById(id);
    //     return View(user);
    // }
    //
    // [HttpPost, ActionName("Delete")]
    // public async Task<IActionResult> DeleteConfirmed(int id)
    // {
    //     await _userService.DeleteUser(id);
    //     return RedirectToAction(nameof(Index));
    // }
    [Authorize(Roles = "NormalUser")]
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }
    [Authorize(Roles = "NormalUser")]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(UserLogin model)
    {
        var userId = int.Parse(HttpContext.Session.GetString("UserId"));
        var user = _userService.GetUserById(userId);
        var oldPasswordHash = HashPassword(model.OldPassword);

        if (oldPasswordHash != user.ToString())
        {
            TempData["Warning"] = "The old password is incorrect.";
            model.RememberMe = false;
            return View(model);
        }
        else
        {
            var newPasswordHash = HashPassword(model.Password);
            await _userService.ChangePassword(userId, newPasswordHash);
            TempData["Message"] = "Your Password changed successful.";
            return RedirectToAction("Index","BorrowingRequest");
        }
    }

    // [Authorize(Roles = "SuperUser")]
    // [HttpPost]
    // public async Task<IActionResult> ChangeAdminPassword(int id, string newPassword)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         await _userService.ChangePassword(id, newPassword);
    //         return RedirectToAction(nameof(Index));
    //     }
    //     return View();
    // }
}
