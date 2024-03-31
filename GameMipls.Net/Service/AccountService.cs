using System.Security.Cryptography;
using System.Text;
using GameMipls.Net.Interface;
using GameMipls.Net.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace GameMipls.Net.Service;

public class AccountService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AccountService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }
    
    public string CreateHash()
    {
        Guid randomGuid = Guid.NewGuid();
        string randomHash = randomGuid.ToString("N");

        return randomHash;
    }

    public async Task<string> CreatePasswordHash(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                builder.Append(hashValue[i].ToString("x2")); // Преобразуем байты хэша в шестнадцатеричное представление
            }

            return builder.ToString();
        }
    }

    public async Task<string> GetName()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity.IsAuthenticated)
        {
            return user.Identity.Name;
        }
        else
        {
            return "";
        }
    }
    
    public async Task<bool> IsAuth()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity.IsAuthenticated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}