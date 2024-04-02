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

    private readonly GameDbContext _gameDbContext;

    public HomeController(ILogger<HomeController> logger, AppDbContext context,
        SignInManager<User> signInManager, AccountService accountService, GameDbContext gameDbContext)
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
        _accountService = accountService;
        _gameDbContext = gameDbContext;
    }
    
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? value)
    {
        if (value == "logout")
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        var user = _context.Users.FirstOrDefault(x => x.Phone == model.Phone);

        user.Name = model.Name;
        user.LastName = model.LastName;
        user.Status = model.Status;
        user.About = model.About;
        user.City = model.City;
        user.Email = model.Email;

        _context.Users.Update(user);
        _context.SaveChanges();
        
        return View(model);
    }
    
    [HttpGet]
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
        model.About = user.About;
        model.Status = user.Status;
        
        return View(model);
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

            var item = _context.Users.FirstOrDefault(x => x.Phone == x.Phone);

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
                return View("Index", model);
            }
            else
            {
                return RedirectToAction("Error", "Home");
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
            Email = ""
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

    [Authorize]
    [HttpGet]
    public IActionResult Create_events()
    {
        return View();
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult Create_events(GameViewModel model)
    {
        TableGame tableGame = new()
        {
            Id = _accountService.CreateHash(),
            Title = "model.TableGame.Title",
            Type = "model.TableGame.Type",
            Description = model.TableGame.Description,
            // MaxPeople = model.TableGame.MaxPeople,
            MaxPeople = 52,
            Announcement = model.TableGame.Announcement,
            Date = model.TableGame.Date,
            Time = model.TableGame.Time,
            Venue = model.TableGame.Venue,
            Price = model.TableGame.Price,
            City = model.TableGame.City,
            IsOnline = model.TableGame.IsOnline,
            // IsFree = model.TableGame.IsFree,
            IsFree = "true"
        };

        _gameDbContext.Tables.Add(tableGame);
        _gameDbContext.SaveChanges();

        GamesViewModel newModel = new()
        {
            Tables = _gameDbContext.Tables.ToList()
        };
        
        return View("Events", newModel);
    }

    [Authorize]
    public IActionResult Events()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}