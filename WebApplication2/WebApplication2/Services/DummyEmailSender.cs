using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebApplication2.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Тут можна додати лог або вивід у консоль, якщо треба
            return Task.CompletedTask;
        }
    }
}