using EcoMeal.API.Entities;
using EcoMeal.API.Application.Models.Auth;
using EcoMeal.API.Application.Constants;
using EcoMeal.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcoMeal.API.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public AuthController(UserManager<User> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name,
            Contact = request.Contact
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });

        await _userManager.AddToRoleAsync(user, UserRoles.User);

        // Send welcome email
        try
        {
            var subject = "Bine ai venit pe EcoMeal!";
            var body = $@"<h3>Salut, {user.Name}!</h3>
                <p>Contul tău EcoMeal a fost creat cu succes.</p>
                <p>Acum poți explora pachetele disponibile și să contribui la reducerea risipei alimentare!</p>
                <p>Cu drag,<br/>Echipa EcoMeal</p>";

            await _emailService.SendEmailAsync(request.Email, subject, body);
        }
        catch
        {
            // Don't fail registration if email fails
        }

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login-notify")]
    public async Task<IActionResult> LoginNotify()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || string.IsNullOrEmpty(user.Email))
            return Ok();

        try
        {
            var subject = "Autentificare nouă pe EcoMeal";
            var body = $@"<h3>Salut, {user.Name}!</h3>
                <p>Te-ai autentificat cu succes pe platforma EcoMeal la data de <b>{DateTime.UtcNow:dd.MM.yyyy HH:mm} UTC</b>.</p>
                <p>Dacă nu tu ai făcut această autentificare, te rugăm să îți schimbi parola imediat.</p>
                <p>Cu drag,<br/>Echipa EcoMeal</p>";

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
        catch
        {
            // Don't fail if email fails
        }

        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new UserMeResponse
        {
            Email = user.Email,
            Name = user.Name,
            Contact = user.Contact,
            Roles = roles
        });
    }
}
