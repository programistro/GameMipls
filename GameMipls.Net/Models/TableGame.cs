using System.Diagnostics.Contracts;

namespace GameMipls.Net.Models;

public class TableGame
{
    public string Type { get; set; }
    
    public string Title { get; set; }
    
    public string Announcement { get; set; }

    private int maxpeople;
    
    public int MaxPeople
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

    private bool isfree;

    public bool IsFree
    {
        get
        {
            return isfree;
        }
        set
        {
            if (value == true)
            {
                Price = 0;
                isfree = value;
            }
        }
    }
    
    public string City { get; set; }
    
    public bool IsOnline { get; set; }

    public string Venue { get; set; }
    
    public DateTime Date { get; set; }
    
    public DateTime Time { get; set; }
    
    public string Description { get; set; }
}