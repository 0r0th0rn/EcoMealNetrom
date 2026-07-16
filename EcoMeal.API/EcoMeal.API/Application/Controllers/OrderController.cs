using System.Security.Claims;
using EcoMeal.API.Application.Services;
using EcoMeal.API.Entities;
using EcoMeal.API.Infrastructure;
using EcoMeal.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoMeal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly EcoMealDbContext _context;
    private readonly IEmailService _emailService;

    public OrderController(EcoMealDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderGetDTO>> PlaceOrder([FromBody] OrderCreateDTO request)
    {
        var userId = GetCurrentUserId();
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var package = await _context.Packages
            .Include(p => p.Business)
            .Include(p => p.Orders)
            .FirstOrDefaultAsync(p => p.Id == request.PackageId);

        if (package is null)
        {
            return NotFound("Pachetul nu a fost gasit");
        }
        if (package.EndRidicare < DateTime.Now)
        {
            return BadRequest("Pachetul nu mai poate fi comandat deoarece timpul de ridicare a expirat.");
        }

        if (package.AvailableQty <= 0)
        {
            return BadRequest("Stoc epuizat. Pachetul nu mai este disponibil.");
        }

        package.AvailableQty--;

        var order = new Order
        {
            UserId = userId,
            PackageId = package.Id,
            Status = "Rezervata",
            Date = DateTime.Now
        };

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        var packageName = package.Name;
        var businessName = package.Business!.Name;
        var businessEmail = "restaurant@test.com";

        _ = Task.Run(async () =>
        {
            try
            {
                if (!string.IsNullOrEmpty(userEmail))
                {
                    var clientSubject = $"Confirmare rezervare - {packageName}";
                    var clientBody = $"<h3>Salut!</h3><p>Comanda ta pentru <b>{packageName}</b> de la <b>{businessName}</b> a fost rezervată cu succes.</p><p>Te așteptăm să o ridici!</p>";

                    await _emailService.SendEmailAsync(userEmail, clientSubject, clientBody);
                }

                if (!string.IsNullOrEmpty(businessEmail))
                {
                    var bizSubject = $"Comandă nouă - {packageName}";
                    var bizBody = $"<h3>O nouă rezervare!</h3><p>Ai o comandă nouă pentru pachetul <b>{packageName}</b>. Te rugăm să îl pregătești.</p>";

                    await _emailService.SendEmailAsync(businessEmail, bizSubject, bizBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la trimiterea e-mailului în background: {ex.Message}");
            }
        });

        return Ok(new OrderGetDTO()
        {
            Id = order.Id,
            Date = order.Date,
            Status = order.Status,
            PackageName = package.Name,
            Price = package.Price,
            BusinessId = package.BusinessId,
            BusinessName = package.Business!.Name,
            UserName = order.User?.Name,
            UserContact = order.User?.Contact
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderGetDTO>>> GetMyOrders()
    {
        var userId = GetCurrentUserId();
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.Date)
            .Select(o => new OrderGetDTO
            {
                Id = o.Id,
                Date = o.Date,
                Status = o.Status,
                Price = o.Package!.Price,
                BusinessId = o.Package.BusinessId,
                BusinessName = o.Package.Business!.Name,
                PackageName = o.Package.Name
            }).ToListAsync();

        return Ok(orders);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<OrderGetDTO>>> GetAllOrders()
    {
        var orders = await _context.Orders
            .OrderByDescending(o => o.Date)
            .Select(o => new OrderGetDTO
            {
                Id = o.Id,
                Date = o.Date,
                Status = o.Status,
                Price = o.Package!.Price,
                BusinessId = o.Package.BusinessId,
                BusinessName = o.Package.Business!.Name,
                PackageName = o.Package.Name,
                UserName = o.User != null ? o.User.Name : null,
                UserContact = o.User != null ? o.User.Contact : null
            }).ToListAsync();

        return Ok(orders);
    }

    [HttpPatch("{id}/completeaza")]
    public async Task<IActionResult> CompleteOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Package)
            .ThenInclude(p => p.Business)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound("Comanda nu a fost gasita.");
        }

        if (order.Status == "Completata")
        {
            return BadRequest("Comanda a fost deja completata.");
        }

        order.Status = "Completata";

        await _context.SaveChangesAsync();

        if (order.User != null && !string.IsNullOrEmpty(order.User.Email))
        {
            var userEmail = order.User.Email;
            var subject = $"Comandă ridicată - {order.Package?.Name}";
            var body = $"<h3>Felicitări!</h3><p>Comanda ta pentru <b>{order.Package?.Name}</b> de la <b>{order.Package?.Business?.Name}</b> a fost ridicată cu succes.</p><p>Sperăm să îți placă!</p>";

            await _emailService.SendEmailAsync(userEmail, subject, body);
        }

        return Ok(new { Message = "Statusul comenzii a fost actualizat la Completata." });
    }

    private int GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userIdValue!);
    }

    [HttpGet("stats")]
    public async Task<ActionResult<UserStatsDTO>> GetMyStats()
    {
        var userId = GetCurrentUserId();

        var userOrders = await _context.Orders
            .Include(o => o.Package)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        double totalWeight = 0;
        decimal totalMoney = 0;

        foreach (var order in userOrders)
        {
            if (order.Package != null)
            {
                totalWeight += order.Package.weightInKg;

                if (order.Package.OldPrice > order.Package.Price)
                {
                    totalMoney += (order.Package.OldPrice - order.Package.Price);
                }
            }
        }

        return Ok(new UserStatsDTO
        {
            TotalOrders = userOrders.Count,
            TotalWeightSaved = totalWeight,
            TotalMoneySaved = totalMoney
        });
    }
}