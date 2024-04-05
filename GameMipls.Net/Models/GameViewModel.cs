using System.ComponentModel.DataAnnotations;

namespace GameMipls.Net.Models;

public class GameViewModel
{
    public TableGame TableGame { get; set; }
    
    public Sport SportGame { get; set; }
    
    public CompGame CompGame { get; set; }
    
    public string TypeGame { get; set; }
    
    public IFormFile? Image { get; set; }
}