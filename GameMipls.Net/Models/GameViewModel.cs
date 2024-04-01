using System.ComponentModel.DataAnnotations;

namespace GameMipls.Net.Models;

public class GameViewModel
{
    [Required] public TableGame TableGame { get; set; }
}