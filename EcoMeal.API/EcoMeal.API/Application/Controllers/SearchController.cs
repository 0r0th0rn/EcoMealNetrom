using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoMeal.API.Infrastructure;
using EcoMeal.API.Application.Models;
using System.Linq;
using System.Threading.Tasks;
using EcoMeal.API.Entities;
using EcoMeal.API.Models;

namespace EcoMeal.API.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly EcoMealDbContext _context;

        public SearchController(EcoMealDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<SearchResultDTO>> GetSearch([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            {
                return Ok(new SearchResultDTO());
            }

            var businesses = await _context.Businesses
                .Where(b => EF.Functions.Like(b.Name, $"%{q}%"))
                .Take(5)
                .Select(b => new BusinessDTO
                {
                    Id = b.Id,
                    Name = b.Name

                })
                .ToListAsync();

            var packages = await _context.Packages
                .Where(p => EF.Functions.Like(p.Name, $"%{q}%"))
                .Take(5)
                .Select(p => new PackageGetDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    BusinessId = p.BusinessId
                })
                .ToListAsync();

            var result = new SearchResultDTO
            {
                Businesses = businesses,
                Packages = packages
            };

            return Ok(result);
        }
    }
}