using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MindSteps.Application.Interfaces;

namespace MindSteps.Application.Services;

public class AsaasService : IAsaasService
{
    private readonly HttpClient _httpClient;
    private readonly string _accessToken;
    private readonly string _baseUrl;

    public AsaasService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _accessToken = configuration["Asaas:AccessToken"] ?? "mock-token";
        _baseUrl = (configuration["Asaas:BaseUrl"] ?? "https://sandbox.asaas.com/api/v3").TrimEnd('/');
    }

    public async Task<string> CreateCustomerAsync(string name, string email, string document, string? phone)
    {
        if (_accessToken == "mock-token")
        {
            return $"cus_mock_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        var requestUrl = $"{_baseUrl}/customers";
        var payload = new
        {
            name = name,
            email = email,
            cpfCnpj = document.Replace(".", "").Replace("-", "").Replace("/", ""),
            phone = phone
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
        request.Headers.Add("access_token", _accessToken);
        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar cliente no Asaas: {errorContent}");
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("id").GetString() ?? throw new Exception("ID do cliente não retornado pelo Asaas");
    }

    public async Task<(string SubscriptionId, string PaymentUrl, string PixCopyPaste)> CreateSubscriptionAsync(string customerId, string plan, double value)
    {
        if (_accessToken == "mock-token")
        {
            var mockSubId = $"sub_mock_{Guid.NewGuid().ToString().Substring(0, 8)}";
            return (mockSubId, "https://sandbox.asaas.com/payment-mock", "mock-pix-copy-paste-code");
        }

        var requestUrl = $"{_baseUrl}/subscriptions";
        var payload = new
        {
            customer = customerId,
            billingType = "UNDEFINED", // Permite cartão, Pix ou boleto
            value = value,
            nextDueDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"), // Vence amanhã
            cycle = "MONTHLY",
            description = $"Assinatura MindSteps - Plano {plan}"
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
        request.Headers.Add("access_token", _accessToken);
        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar assinatura no Asaas: {errorContent}");
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var subscriptionId = doc.RootElement.GetProperty("id").GetString() ?? throw new Exception("ID da assinatura não retornado pelo Asaas");

        // Agora, vamos obter o link de pagamento do primeiro pagamento gerado para essa assinatura
        var paymentsUrl = $"{_baseUrl}/payments?subscription={subscriptionId}";
        using var paymentsRequest = new HttpRequestMessage(HttpMethod.Get, paymentsUrl);
        paymentsRequest.Headers.Add("access_token", _accessToken);

        var paymentsResponse = await _httpClient.SendAsync(paymentsRequest);
        if (paymentsResponse.IsSuccessStatusCode)
        {
            var paymentsJson = await paymentsResponse.Content.ReadAsStringAsync();
            using var paymentsDoc = JsonDocument.Parse(paymentsJson);
            if (paymentsDoc.RootElement.TryGetProperty("data", out var dataElement) && dataElement.GetArrayLength() > 0)
            {
                var firstPayment = dataElement[0];
                var invoiceUrl = firstPayment.TryGetProperty("invoiceUrl", out var urlProp) ? urlProp.GetString() : null;
                var bankSlipUrl = firstPayment.TryGetProperty("bankSlipUrl", out var slipProp) ? slipProp.GetString() : null;
                var paymentUrl = invoiceUrl ?? bankSlipUrl ?? "https://asaas.com";
                
                // Tenta obter o pix copy paste se disponível
                var paymentId = firstPayment.GetProperty("id").GetString();
                var pixUrl = $"{_baseUrl}/payments/{paymentId}/pixQrCode";
                using var pixRequest = new HttpRequestMessage(HttpMethod.Get, pixUrl);
                pixRequest.Headers.Add("access_token", _accessToken);
                var pixResponse = await _httpClient.SendAsync(pixRequest);
                string pixCopyPaste = "";
                if (pixResponse.IsSuccessStatusCode)
                {
                    var pixJson = await pixResponse.Content.ReadAsStringAsync();
                    using var pixDoc = JsonDocument.Parse(pixJson);
                    if (pixDoc.RootElement.TryGetProperty("payload", out var payloadProp))
                    {
                        pixCopyPaste = payloadProp.GetString() ?? "";
                    }
                }

                return (subscriptionId, paymentUrl, pixCopyPaste);
            }
        }

        return (subscriptionId, "https://asaas.com", "");
    }
}
