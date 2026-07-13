using System.Security.Claims;
using EcoMeal.API.Entities;
using EcoMeal.API.Infrastructure;
using EcoMeal.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EcoMeal.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly EcoMealDbContext _context;
    public OrderController(EcoMealDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<ActionResult<OrderGetDTO>> PlaceOrder([FromBody] OrderCreateDTO request)
    {
        var userId = GetCurrentUserId();

        var package = await _context.Packages
            .Include(p => p.Business)
            .Include(p => p.Orders)
            .FirstOrDefaultAsync(p => p.Id == request.PackageId);
        if (package is null)
        {
            return NotFound("Pachetul nu a fost gasit");
        }

        if (package.Orders.Any())
        {
            return BadRequest("Pachetul nu mai e disponibil");
        }

        var order = new Order
        {
            UserId = userId,
            PackageId = package.Id,
            Status = "Plasate",
            Date = DateTime.UtcNow
        };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
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
        }
        );
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
    private int GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userIdValue!);
    }
}