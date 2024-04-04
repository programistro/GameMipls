using GameMipls.Net.Models;
using Microsoft.EntityFrameworkCore;

namespace GameMipls.Net.Data;

public class GameDbContext : DbContext
{
    public DbSet<TableGame> Tables => Set<TableGame>();

    public DbSet<User> Users => Set<User>();
    
    // public DbSet<ComputerGame> ComputerGames => Set<ComputerGame>();
    //
    // public DbSet<Sport> Sports => Set<Sport>();
    
    public GameDbContext() => Database.EnsureCreated();
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=games.db");
    }
}