using EcoMeal.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoMeal.API.Application;

[ApiController]
[Route("api/[controller]")]
public class PackageTypeController : ControllerBase
{
    private readonly EcoMealDbContext _context;
    public PackageTypeController(EcoMealDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll()
    {
        var packageTypes = await _context.PackageTypes
            .Select(pt => new { pt.Id, pt.Name })
            .ToListAsync();
        return Ok(packageTypes);
    }
}
