using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace GameMipls.Net.Controllers;

[Microsoft.AspNetCore.Mvc.Route("/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("google-login")]
    public async Task<ActionResult> Google()
    {
        var prop = new AuthenticationProperties
        {
            RedirectUri = "/"
        };
        return Challenge(prop, GoogleDefaults.AuthenticationScheme);
    }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [AllowAnonymous]
    [HttpGet("signout")]
    public async Task<ActionResult> signout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var prop = new AuthenticationProperties
        {
            RedirectUri = "/logout-complete"
        };

        return Redirect("/");
    }

    [HttpGet("logout-complete")]
    [AllowAnonymous]
    public string logoutComplete()
    {
        return "logout-complete";
    }

    public IActionResult YourAction()
    {
        // Выполняем навигацию на другую страницу с помощью метода NavigateTo()
        NavigationManager.NavigateTo("/");

        // Если требуется вернуть результат обратно на страницу,
        // можно использовать метод Redirect()
        return Redirect("/");
    }

    [HttpGet("singin")]
    public async Task<ActionResult> Singin(string email)
    {
        // создаем один claim
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, email)
        };
        // создаем объект ClaimsIdentity
        ClaimsIdentity id = new ClaimsIdentity(claims, "LoginScheme", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        // установка аутентификационных куки
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        var prop = new AuthenticationProperties
        {
            RedirectUri = "/"
        };
        return Redirect("/");
    }
}