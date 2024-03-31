using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GameMipls.Net.Data;
using Microsoft.AspNetCore.Mvc;
using GameMipls.Net.Models;
using GameMipls.Net.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GameMipls.Net.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly AppDbContext _context;
    
    private readonly SignInManager<User> _signInManager;

    private readonly AccountService _accountService;

    public HomeController(ILogger<HomeController> logger, AppDbContext context,
        SignInManager<User> signInManager, AccountService accountService)
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
        _accountService = accountService;
    }
    
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? value)
    {
        if (value == "logout")
        {
            _signInManager.SignOutAsync();
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    [Authorize]
    [Route("Home/profile")]
    public async Task<IActionResult> Setting_profile_user(LoginViewModel model)
    {
        var name = _signInManager.Context.User.Identity.Name;

        var user = _context.Users.FirstOrDefault(x => x.Name == name);

        model.Name = user.Name;
        model.LastName = user.LastName;
        model.Password = user.Password;
        model.Phone = user.Phone;
        model.Email = user.Email;
        
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Reg()
    {
        return View();
    }

    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (_context.Users.Any(x => x.Phone == model.Phone))
        {
            string hash = await _accountService.CreatePasswordHash(model.Password);
            
            // var result = await _signInManager.PasswordSignInAsync(model.Name, hash, true, lockoutOnFailure: false);

            var item = _context.Users.FirstOrDefault(x => x.Phone == x.Phone);

            var result = _signInManager.SignInAsync(item, true);

            model.Name = item.Name;
            model.LastName = item.LastName;
            
            if (result.IsCompleted)
            {
                // Вход выполнен успешно
                return View("Setting_profile_user", model);
            }
            else
            {
                return View("Error");
            }
        }
        else
        {
            return View("Error");
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Reg(LoginViewModel model)
    {
        User user = new()
        {
            Id = _accountService.CreateHash(),
            PhoneNumber = model.Phone,
            EmailConfirmed = false,
            UserName = model.Name,
            Name = model.Name,
            LastName = model.LastName,
            Password = await _accountService.CreatePasswordHash(model.Password),
            Phone = model.Phone
        };
        var item = _context.Users.Any(x => x.Phone == user.Phone);

        if (item)
        {
            return View("Error");
        }
        _context.Users.Add(user);
        _context.SaveChanges();
            
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(ClaimTypes.Role, "user")
            // Добавьте другие необходимые клеймы
        };

        var identity = new ClaimsIdentity(claims, "Cookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("Cookie", principal, new AuthenticationProperties
        {
            ExpiresUtc = DateTime.UtcNow.AddMinutes(120), // Установка времени истечения куки
            IsPersistent = true, // Установка постоянности куки
        });
            
        return View("Setting_profile_user", model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}