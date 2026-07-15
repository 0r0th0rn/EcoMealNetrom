using System.Net;
using System.Net.Mail;
using Microsoft.Identity.Client;

namespace EcoMeal.API.Application.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;
    public SmtpEmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var host = _config["SmtpSettings:Host"];
        var port = int.Parse(_config["SmtpSettings:Port"]);
        var username = _config["SmtpSettings:Username"];
        var password = _config["SmtpSettings:Password"];

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_config["SmtpSettings:FromEmail"], "EcoMeal App"),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        mailMessage.To.Add(toEmail);
        await client.SendMailAsync(mailMessage);
    }
}