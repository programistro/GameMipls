using System.ComponentModel.DataAnnotations;

namespace GameMipls.Net.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ошибка")]
    [EmailAddress(ErrorMessage = "Ошибка")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Ошибка")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    [Required]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Ошибка")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }
    
    public string? About { get; set; }
    
    public string? Status { get; set; }
    
    public string? City { get; set; }
    
    public IFormFile? Image { get; set; }
    
    public string PathToImage { get; set; }
}
