using System.Security.Claims;
using GameMipls.Net.Data;
using GameMipls.Net.Models;
using GameMipls.Net.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameMipls.Net.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    
    private readonly AppDbContext _context;
    
    private readonly SignInManager<User> _signInManager;

    private readonly AccountService _accountService;

    public AuthController(ILogger<AuthController> logger, AppDbContext context,
        SignInManager<User> signInManager, AccountService accountService)
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
        _accountService = accountService;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Auth()
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

            var item = _context.Users.FirstOrDefault(x => x.Phone == model.Phone);

            if (item.Password == hash)
            {
                var result = _signInManager.SignInAsync(item, true);
            
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, item.Name),
                    new Claim(ClaimTypes.MobilePhone, item.Phone),
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

                model.Name = item.Name;
                model.LastName = item.LastName;
            
                if (result.IsCompleted)
                {
                    // Вход выполнен успешно
                    return RedirectToAction("Index", "Home", model);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
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
    public async Task<IActionResult> Auth(LoginViewModel model)
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
            Phone = model.Phone,
            City = "",
            About = "",
            Status = "",
            Email = "",
            Image = ""
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
     
        return RedirectToAction("Index", "Home");
        //return View("Setting_profile_user", model);
    }
}