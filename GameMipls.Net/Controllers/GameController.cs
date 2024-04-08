using GameMipls.Net.Data;
using GameMipls.Net.Models;
using GameMipls.Net.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameMipls.Net.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly ILogger<GameController> _logger;
    
    private readonly AppDbContext _context;
    
    private readonly SignInManager<User> _signInManager;

    private readonly GameDbContext _gameDbContext;

    private readonly AccountService _accountService;

    public GameController(ILogger<GameController> logger, AppDbContext context,
        SignInManager<User> signInManager, AccountService accountService, GameDbContext gameDbContext)
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
        _accountService = accountService;
        _gameDbContext = gameDbContext;
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult Create_events()
    {
        return View();
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult Create_events(GameViewModel model)
    {
        TableGame tableGame = new()
        {
            Id = _accountService.CreateHash(),
            Title = "model.TableGame.Title",
            Type = "model.TableGame.Type",
            Description = model.TableGame.Description,
            // MaxPeople = model.TableGame.MaxPeople,
            MaxPeople = 52,
            Announcement = model.TableGame.Announcement,
            Date = model.TableGame.Date,
            Time = model.TableGame.Time,
            Venue = model.TableGame.Venue,
            Price = model.TableGame.Price,
            City = model.TableGame.City,
            IsOnline = model.TableGame.IsOnline,
            // IsFree = model.TableGame.IsFree,
            IsFree = "true",
            IsEdit = "true",
            Owner = model.TableGame.Owner
        };

        _gameDbContext.Tables.Add(tableGame);
        _gameDbContext.SaveChanges();

        GamesViewModel newModel = new()
        {
            Tables = _gameDbContext.Tables.ToList()
        };
        
        return View("Events", newModel);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Events(GamesViewModel model)
    {
        model.Tables = _gameDbContext.Tables.Where(x => x.Owner == _signInManager.Context.User.Identity.Name).ToList();
        model.Sports = _gameDbContext.Sports.Where(x => x.Owner == _signInManager.Context.User.Identity.Name).ToList();
        model.CompGames = _gameDbContext.ComputerGame.Where(x => x.Owner == _signInManager.Context.User.Identity.Name).ToList();
        model.Users = _gameDbContext.Users.ToList();
        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Events(GameViewModel model)
    {
        if (model.TypeGame == "Настольные игры")
        {
            TableGame game = new()
            {
                Id = _accountService.CreateHash(),
                Title = model.TableGame.Title,
                Announcement = model.TableGame.Announcement,
                Date = model.TableGame.Date,
                Time = model.TableGame.Time,
                City = model.TableGame.City,
                IsOnline = model.TableGame.IsOnline,
                IsFree = model.TableGame.IsFree,
                Description = model.TableGame.Description,
                MaxPeople = model.TableGame.MaxPeople,
                Price = model.TableGame.Price,
                Type = model.TypeGame,
                Sort = model.TableGame.Sort,
                IsEdit = "true",
                Venue = model.TableGame.Venue,
                View = 0,
                Registrations = 0,
                PaymentDeadline = 0,
                Owner = _signInManager.Context.User.Identity.Name,
            };

            if (model.Image != null)
            {
                var filename = Path.GetFileName(model.Image.FileName);
                var path = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/avatar", "", filename);
        
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                string pathImage = $"{_accountService.CreateHash()}.png";
                _accountService.CropToCircle(path, 710,400, pathImage);

                game.PathToImage = $"{pathImage}";
            }
            else
            {
                game.PathToImage = "";
            }
        
            _gameDbContext.Tables.Add(game);

            _gameDbContext.SaveChanges();
        }
        else if (model.TypeGame == "Компьютерные игры")
        {
            CompGame game = new()
            {
                Id = _accountService.CreateHash(),
                Title = model.TableGame.Title,
                Announcement = model.TableGame.Announcement,
                Date = model.TableGame.Date,
                Time = model.TableGame.Time,
                City = model.TableGame.City,
                IsOnline = model.TableGame.IsOnline,
                IsFree = model.TableGame.IsFree,
                Description = model.TableGame.Description,
                MaxPeople = model.TableGame.MaxPeople,
                Price = model.TableGame.Price,
                Type = model.TypeGame,
                Sort = model.TableGame.Sort,
                IsEdit = "true",
                Venue = model.TableGame.Venue,
                View = 0,
                Registrations = 0,
                PaymentDeadline = 0,
                Owner = _signInManager.Context.User.Identity.Name,
            };

            if (model.Image != null)
            {
                var filename = Path.GetFileName(model.Image.FileName);
                var path = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/avatar", "", filename);
        
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                string pathImage = $"{_accountService.CreateHash()}.png";
                _accountService.CropToCircle(path, 710,400, pathImage);

                game.PathToImage = $"{pathImage}";
            }
            else
            {
                game.PathToImage = "";
            }
        
            _gameDbContext.ComputerGame.Add(game);

            _gameDbContext.SaveChanges();
        }
        else if (model.TypeGame == "Спорт")
        {
            Sport game = new()
            {
                Id = _accountService.CreateHash(),
                Title = model.TableGame.Title,
                Announcement = model.TableGame.Announcement,
                Date = model.TableGame.Date,
                Time = model.TableGame.Time,
                City = model.TableGame.City,
                IsOnline = model.TableGame.IsOnline,
                IsFree = model.TableGame.IsFree,
                Description = model.TableGame.Description,
                MaxPeople = model.TableGame.MaxPeople,
                Price = model.TableGame.Price,
                Type = model.TypeGame,
                Sort = model.TableGame.Sort,
                IsEdit = "true",
                Venue = model.TableGame.Venue,
                View = 0,
                Registrations = 0,
                PaymentDeadline = 0,
                Owner = _signInManager.Context.User.Identity.Name,
            };

            if (model.Image != null)
            {
                var filename = Path.GetFileName(model.Image.FileName);
                var path = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/avatar", "", filename);
        
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                string pathImage = $"{_accountService.CreateHash()}.png";
                _accountService.CropToCircle(path, 710,400, pathImage);

                game.PathToImage = $"{pathImage}";
            }
            else
            {
                game.PathToImage = "";
            }
        
            _gameDbContext.Sports.Add(game);

            _gameDbContext.SaveChanges();
        }

        
        GamesViewModel games = new GamesViewModel();
        games.Tables = _gameDbContext.Tables.ToList();
        games.Sports = _gameDbContext.Sports.ToList();
        games.CompGames = _gameDbContext.ComputerGame.ToList();
        
        return RedirectToAction("Events", games);
        
        // return View("Events", games);
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult Games_events(GamesViewModel model, string? filter)
    {
        if (filter != null)
        {
            if (filter == "table")
            {
                model.Tables = _gameDbContext.Tables.ToList();
                return View(model);
            }
            else if (filter == "comp")
            {
                model.CompGames = _gameDbContext.ComputerGame.ToList();
                return View(model);
            }
            else if (filter == "sport")
            {
                model.Sports = _gameDbContext.Sports.ToList();
                return View(model);
            }
        }
        
        model.Tables = _gameDbContext.Tables.ToList();
        model.Users = _gameDbContext.Users.ToList();
        model.Sports = _gameDbContext.Sports.ToList();
        model.CompGames = _gameDbContext.ComputerGame.ToList();
        return View(model);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Editing_event(string id, string typeGame)
    {
        if (id != null)
        {
            GameVM newModel = new GameVM();
            
            if (typeGame == "Настольные игры")
            {
                var model = _gameDbContext.Tables.FirstOrDefault(x => x.Id == id);
                
                newModel.Announcement = model.Announcement;
                newModel.Description = model.Description;
                newModel.Date = model.Date;
                newModel.Owner = model.Owner;
                newModel.Price = model.Price;
                newModel.MaxPeople = model.MaxPeople;
                newModel.Sort = model.Sort;
                newModel.IsOnline = model.IsOnline;
                newModel.Time = model.Time;
                newModel.IsFree = model.IsFree;
                newModel.Title = model.Title;
                newModel.City = model.City;
                newModel.Type = model.Type;
                newModel.View = model.View;
                newModel.Venue = model.Venue;
                newModel.Id = model.Id;
                newModel.PaymentDeadline = model.PaymentDeadline;
                newModel.PathToImage = model.PathToImage;
                newModel.Registrations = model.Registrations;
            
                return View("Editing_event", newModel);
            }
            else if (typeGame == "Спорт")
            {
                var model = _gameDbContext.Sports.FirstOrDefault(x => x.Id == id);
                
                newModel.Announcement = model.Announcement;
                newModel.Description = model.Description;
                newModel.Date = model.Date;
                newModel.Owner = model.Owner;
                newModel.Price = model.Price;
                newModel.MaxPeople = model.MaxPeople;
                newModel.Sort = model.Sort;
                newModel.IsOnline = model.IsOnline;
                newModel.Time = model.Time;
                newModel.IsFree = model.IsFree;
                newModel.Title = model.Title;
                newModel.City = model.City;
                newModel.Type = model.Type;
                newModel.View = model.View;
                newModel.Venue = model.Venue;
                newModel.Id = model.Id;
                newModel.PaymentDeadline = model.PaymentDeadline;
                newModel.PathToImage = model.PathToImage;
                newModel.Registrations = model.Registrations;
            
                return View("Editing_event", newModel);
            }
            else if (typeGame == "Компьютерные игры")
            {
                var model = _gameDbContext.ComputerGame.FirstOrDefault(x => x.Id == id);
                
                newModel.Announcement = model.Announcement;
                newModel.Description = model.Description;
                newModel.Date = model.Date;
                newModel.Owner = model.Owner;
                newModel.Price = model.Price;
                newModel.MaxPeople = model.MaxPeople;
                newModel.Sort = model.Sort;
                newModel.IsOnline = model.IsOnline;
                newModel.Time = model.Time;
                newModel.IsFree = model.IsFree;
                newModel.Title = model.Title;
                newModel.City = model.City;
                newModel.Type = model.Type;
                newModel.View = model.View;
                newModel.Venue = model.Venue;
                newModel.Id = model.Id;
                newModel.PaymentDeadline = model.PaymentDeadline;
                newModel.PathToImage = model.PathToImage;
                newModel.Registrations = model.Registrations;
            
                return View("Editing_event", newModel);
            }
        }
        return View("Error");
    }

    [Authorize]
    [HttpPost]
    public IActionResult Editing_event(GameVM model)
    {
        if (model.Type == "Настольные игры")
        {
            var item = _gameDbContext.Tables.FirstOrDefault(x => x.Id == model.Id);
            
            item.Announcement = model.Announcement != null ? model.Announcement : item.Announcement;
            item.Description = model.Description != null ? model.Description : item.Description;
            item.Date = model.Date != null ? model.Date : item.Date;
            item.Owner = model.Owner != null ? model.Owner : item.Owner;
            item.Price = model.Price != null ? model.Price : item.Price;
            item.MaxPeople = model.MaxPeople != null ? model.MaxPeople : item.MaxPeople;
            item.Sort = model.Sort != null ? model.Sort : item.Sort;
            item.IsOnline = model.IsOnline != null ? model.IsOnline : item.IsOnline;
            item.Time = model.Time != null ? model.Time : item.Time;
            item.IsFree = model.IsFree != null ? model.IsFree : item.IsFree;
            item.Title = model.Title != null ? model.Title : item.Title;
            item.City = model.City != null ? model.City : item.City;
            item.Type = model.Type != null ? model.Type : item.Type;
            item.View = model.View != null ? model.View : item.View;
            item.Venue = model.Venue != null ? model.Venue : item.Venue;
            item.Id = model.Id != null ? model.Id : item.Id;
            item.PaymentDeadline = model.PaymentDeadline != null ? model.PaymentDeadline : item.PaymentDeadline;
            item.PathToImage = model.PathToImage != null ? model.PathToImage : item.PathToImage;
            item.Registrations = model.Registrations != null ? model.Registrations : item.Registrations;
            
            _gameDbContext.Tables.Update(item);
            _gameDbContext.SaveChanges();
        }
        else if (model.Type == "Спорт")
        {
            var item = _gameDbContext.Sports.FirstOrDefault(x => x.Id == model.Id);

            item.Announcement = model.Announcement != null ? model.Announcement : item.Announcement;
            item.Description = model.Description != null ? model.Description : item.Description;
            item.Date = model.Date != null ? model.Date : item.Date;
            item.Owner = model.Owner != null ? model.Owner : item.Owner;
            item.Price = model.Price != null ? model.Price : item.Price;
            item.MaxPeople = model.MaxPeople != null ? model.MaxPeople : item.MaxPeople;
            item.Sort = model.Sort != null ? model.Sort : item.Sort;
            item.IsOnline = model.IsOnline != null ? model.IsOnline : item.IsOnline;
            item.Time = model.Time != null ? model.Time : item.Time;
            item.IsFree = model.IsFree != null ? model.IsFree : item.IsFree;
            item.Title = model.Title != null ? model.Title : item.Title;
            item.City = model.City != null ? model.City : item.City;
            item.Type = model.Type != null ? model.Type : item.Type;
            item.View = model.View != null ? model.View : item.View;
            item.Venue = model.Venue != null ? model.Venue : item.Venue;
            item.Id = model.Id != null ? model.Id : item.Id;
            item.PaymentDeadline = model.PaymentDeadline != null ? model.PaymentDeadline : item.PaymentDeadline;
            item.PathToImage = model.PathToImage != null ? model.PathToImage : item.PathToImage;
            item.Registrations = model.Registrations != null ? model.Registrations : item.Registrations;
            
            _gameDbContext.Sports.Update(item);
            _gameDbContext.SaveChanges();
        }
        else if (model.Type == "Компьютерные игры")
        {
            var item = _gameDbContext.ComputerGame.FirstOrDefault(x => x.Id == model.Id);

            item.Announcement = model.Announcement != null ? model.Announcement : item.Announcement;
            item.Description = model.Description != null ? model.Description : item.Description;
            item.Date = model.Date != null ? model.Date : item.Date;
            item.Owner = model.Owner != null ? model.Owner : item.Owner;
            item.Price = model.Price != null ? model.Price : item.Price;
            item.MaxPeople = model.MaxPeople != null ? model.MaxPeople : item.MaxPeople;
            item.Sort = model.Sort != null ? model.Sort : item.Sort;
            item.IsOnline = model.IsOnline != null ? model.IsOnline : item.IsOnline;
            item.Time = model.Time != null ? model.Time : item.Time;
            item.IsFree = model.IsFree != null ? model.IsFree : item.IsFree;
            item.Title = model.Title != null ? model.Title : item.Title;
            item.City = model.City != null ? model.City : item.City;
            item.Type = model.Type != null ? model.Type : item.Type;
            item.View = model.View != null ? model.View : item.View;
            item.Venue = model.Venue != null ? model.Venue : item.Venue;
            item.Id = model.Id != null ? model.Id : item.Id;
            item.PaymentDeadline = model.PaymentDeadline != null ? model.PaymentDeadline : item.PaymentDeadline;
            item.PathToImage = model.PathToImage != null ? model.PathToImage : item.PathToImage;
            item.Registrations = model.Registrations != null ? model.Registrations : item.Registrations;

            _gameDbContext.ComputerGame.Update(item);
            _gameDbContext.SaveChanges();
        }
        
        GamesViewModel models = new();
        models.Tables = _gameDbContext.Tables.Where(x => x.Owner == _signInManager.Context.User.Identity.Name).ToList();
        models.Sports = _gameDbContext.Sports.Where(x => x.Owner == _signInManager.Context.User.Identity.Name).ToList();
        models.CompGames = _gameDbContext.ComputerGame.Where(x => x.Owner == _signInManager.Context.User.Identity.Name).ToList();
        models.Users = _gameDbContext.Users.ToList();
        return RedirectToAction("Events", models);
    }
}