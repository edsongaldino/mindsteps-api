using System.Threading.Tasks;

namespace MindSteps.Application.Interfaces;

public interface IAsaasService
{
    Task<string> CreateCustomerAsync(string name, string email, string document, string? phone);
    Task<(string SubscriptionId, string PaymentUrl, string PixCopyPaste)> CreateSubscriptionAsync(string customerId, string plan, double value);
}
