using System;
using System.Threading.Tasks;
using MindSteps.Application.Interfaces;

namespace MindSteps.Infrastructure.Services
{
    public class MockEmailService : IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"[EMAIL MOCK] Enviando para: {to}");
            Console.WriteLine($"[EMAIL MOCK] Assunto: {subject}");
            Console.WriteLine($"[EMAIL MOCK] Corpo:\n{body}");
            Console.WriteLine("-------------------------------------------------");
            
            return Task.CompletedTask;
        }
    }
}
