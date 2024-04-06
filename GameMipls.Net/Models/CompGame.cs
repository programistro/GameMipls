namespace GameMipls.Net.Models;

public class CompGame : IGame
{
    public string Id { get; set; }
    
    public string? Sort { get; set; }

    public string? Type { get; set; }
    
    public string? Title { get; set; }
    
    public string Announcement { get; set; }
    
    public int? MaxPeople { get; set; }
    
    public int Price { get; set; }
    
    public string? PathToImage { get; set; }
    
    public string City { get; set; }
    
    public string? IsOnline { get; set; }
    
    public string Venue { get; set; }
    
    public DateTime Date { get; set; }
    
    public DateTime Time { get; set; }
    
    public int View { get; set; }
    
    public int Registrations { get; set; }
    
    public int PaymentDeadline { get; set; }
    
    public string IsEdit { get; set; }
    
    public string Description { get; set; }
    
    public string Owner { get; set; }
    
    private string? isfree;
    
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
}