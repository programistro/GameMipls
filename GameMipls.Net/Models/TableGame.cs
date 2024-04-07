using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace GameMipls.Net.Models;

public class TableGame : IGame
{
    [Key]
    public string Id { get; set; }

    public string? Sort { get; set; }

    public string? Type { get; set; }
    
    public string? Title { get; set; }
    
    public string Announcement { get; set; }

    private int? maxpeople;
    
    public int? MaxPeople
    {
        get
        {
            return maxpeople;
        }
        set
        {
            maxpeople = value;
        }
    }
    
    public int Price { get; set; }

    private string? isfree;
    
    public string? PathToImage { get; set; }

    public string? IsFree
    {
        get
        {
            return isfree;
        }
        set
        {
            if (value == "true")
            {
                Price = 0;
                isfree = value;
            }
        }
    }
    
    public string City { get; set; }

    private string? isOnline;

    public string? IsOnline
    {
        get
        {
            return isOnline;
        }
        set
        {
            if (isOnline == "true")
            {
                City = "online";
                isOnline = value;
            }
        }
    }

    public string Venue { get; set; }
    
    public DateTime Date { get; set; }
    
    public DateTime Time { get; set; }
    
    public int View { get; set; }
    
    public int Registrations { get; set; }
    
    public int PaymentDeadline { get; set; }
    
    public string IsEdit { get; set; }
    
    public string Description { get; set; }
    
    public string Owner { get; set; }
}