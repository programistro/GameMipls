using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GameMipls.Net.Models;

public class User : IdentityUser
{
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    public string LastName { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }
    
    public string? About { get; set; }
    
    public string? Status { get; set; }
    
    public string? City { get; set; }
}