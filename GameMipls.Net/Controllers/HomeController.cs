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

        if (model.Image != null)
        {
            var filename = Path.GetFileName(model.Image.FileName);
            var path = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/avatar", "", filename);
        
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            string pathImage = $"{_accountService.CreateHash()}.png";
            _accountService.CropToCircle(path, 149,148, pathImage);

            user.Image = $"{pathImage}";
        }
        else
        {
            user.Image = "";
        }
        
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
        model.PathToImage = user.Image;
        
        return View(model);
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}