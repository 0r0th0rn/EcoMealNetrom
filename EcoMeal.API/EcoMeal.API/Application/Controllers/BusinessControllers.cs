using EcoMeal.API.Entities;
using EcoMeal.API.Infrastructure;
using EcoMeal.API.Models;
using Microsoft.AspNetCore.Components.Routing;
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
    [HttpGet("{id}")]
    public async Task<ActionResult<BusinessDetailsDTO>> GetOneById(int id)
    {
        var business = await _context.Businesses
            .Include(b => b.Packages)
            .ThenInclude(p => p.PackageType)
            .Select(b => new BusinessDetailsDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                Description = b.Description,
                Contact = b.Contact,
                BusinessTypeName = b.BusinessType.Name,
                Packages = b.Packages.Select(p => new PackageGetDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StartPickup = p.StartRidicare,
                    EndPickup = p.EndRidicare,
                    PackageTypeName = p.PackageType != null ? p.PackageType.Name : ""
                }).ToList()
            })
            .FirstOrDefaultAsync(b => b.Id == id);
        if (business is null)
        {
            return NotFound();
        }

        return Ok(business);
    }

    [HttpPost]
    [Route("{id}/addPackage")]
    public async Task<IActionResult> AddPackageToBusiness(int id, [FromBody] PackageAddDTO package)
    {
        _context.Packages.Add(new Package
        {
            Name = package.Name,
            Description = package.Description,
            Price = package.Price,
            StartRidicare = package.StartPickup,
            EndRidicare = package.EndPickup,
            PackageTypeId = package.PackageTypeId,
            BusinessId = id,
        });

        await _context.SaveChangesAsync();

        return Created();
    }

    [HttpGet("package/{packageId}")]
    public async Task<ActionResult<PackageAddDTO>> GetPackageById(int packageId)
    {
        var package = await _context.Packages.FindAsync(packageId);
        if (package is null)
            return NotFound();
#pragma warning disable CS8601 // Possible null reference assignment.
        return Ok(new PackageAddDTO
        {
            Name = package.Name,
            Description = package.Description,
            Price = package.Price,
            StartPickup = package.StartRidicare,
            EndPickup = package.EndRidicare,
            PackageTypeId = package.PackageTypeId
        });
#pragma warning restore CS8601 // Possible null reference assignment.
    }
    [HttpPut("package/{packageId}")]
    public async Task<IActionResult> EditPackage(int packageId, [FromBody] PackageAddDTO packageDto)
    {
        var package = await _context.Packages.FindAsync(packageId);
        if (package is null)
            return NotFound();
        package.Name = packageDto.Name;
        package.Description = packageDto.Description;
        package.Price = packageDto.Price;
        package.StartRidicare = packageDto.StartPickup;
        package.EndRidicare = packageDto.EndPickup;
        package.PackageTypeId = packageDto.PackageTypeId;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("package/{packageId}")]
    public async Task<IActionResult> DeletePackage(int packageId)
    {
        var package = await _context.Packages.FindAsync(packageId);
        if (package is null)
            return NotFound();
        _context.Packages.Remove(package);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpPost]
    public async Task<IActionResult> AddBusiness([FromBody] BusinessAddDTO businessDto)
    {
        var business = new Business
        {
            Name = businessDto.Name,
            Address = businessDto.Address,
            Description = businessDto.Description,
            Contact = businessDto.Contact,
            BusinessTypeId = businessDto.BusinessTypeId,
            BusinessType = null!
        };

        _context.Businesses.Add(business);
        await _context.SaveChangesAsync();
        return Created();
    }
    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<object>>> GetBusinessTypes()
    {
        var types = await _context.BusinessTypes
            .Select(bt => new
            {
                Id = bt.Id,
                Name = bt.Name
            })
            .ToListAsync();

        return Ok(types);
    }
    [HttpGet("{id}/edit")]
    public async Task<ActionResult<BusinessAddDTO>> GetBusinessForEdit(int id)
    {
        var business = await _context.Businesses.FindAsync(id);

        if (business == null)
        {
            return NotFound();
        }

        return Ok(new BusinessAddDTO
        {
            Name = business.Name,
            Address = business.Address,
            Contact = business.Contact,
            Description = business.Description,
            BusinessTypeId = business.BusinessTypeId
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditBusiness(int id, [FromBody] BusinessAddDTO businessDto)
    {
        var business = await _context.Businesses.FindAsync(id);

        if (business == null)
        {
            return NotFound();
        }

        business.Name = businessDto.Name;
        business.Address = businessDto.Address;
        business.Contact = businessDto.Contact;
        business.Description = businessDto.Description;
        business.BusinessTypeId = businessDto.BusinessTypeId;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}

