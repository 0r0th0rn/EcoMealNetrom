using EcoMeal.API.Entities;
using EcoMeal.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoMeal.API.Application;

[ApiController]
[Route("api/[controller]")]
public class BusinessController : ControllerBase
{
    private readonly EcoMealDbContext _context;
    public BusinessController(EcoMealDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BusinessDTO>>> GetAll()
    {
        var businessesDTOs = await _context.Businesses
            .Include(b => b.BusinessType)
            .Select(b => new BusinessDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                Description = b.Description,
                Contact = b.Contact,
                BusinessTypeName = b.BusinessType.Name
            }).ToListAsync();
        return Ok(businessesDTOs);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var business = await _context.Businesses
                .Include(b => b.Packages)
                    .ThenInclude(p => p.Orders)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (business is null)
            {
                return NotFound();
            }

            // Remove orders first, then packages, then the business
            foreach (var package in business.Packages)
            {
                _context.Orders.RemoveRange(package.Orders);
            }
            _context.Packages.RemoveRange(business.Packages);
            _context.Businesses.Remove(business);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = ex.Message,
                inner = ex.InnerException?.Message
            });
        }
    }

}