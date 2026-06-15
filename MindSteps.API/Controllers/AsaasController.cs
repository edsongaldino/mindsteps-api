using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.Interfaces;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AsaasController : ControllerBase
{
    private readonly IPsicologoService _psicologoService;

    public AsaasController(IPsicologoService psicologoService)
    {
        _psicologoService = psicologoService;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook([FromBody] JsonElement payload)
    {
        try
        {
            // Log simples para fins de depuração
            Console.WriteLine($"Webhook recebido do Asaas: {payload.GetRawText()}");

            if (!payload.TryGetProperty("event", out var eventProp))
            {
                return BadRequest(new { message = "Evento não especificado no payload" });
            }

            var eventName = eventProp.GetString();

            if (!payload.TryGetProperty("payment", out var paymentProp))
            {
                // Evento pode não ser relacionado a pagamento direto, mas para assinaturas
                return Ok();
            }

            if (!paymentProp.TryGetProperty("subscription", out var subProp) || subProp.ValueKind == JsonValueKind.Null)
            {
                // Pagamento avulso sem assinatura vinculada, ignora ou processa se necessário
                return Ok();
            }

            var subscriptionId = subProp.GetString();
            if (string.IsNullOrEmpty(subscriptionId))
            {
                return Ok();
            }

            bool isPaidEvent = eventName == "PAYMENT_RECEIVED" || eventName == "PAYMENT_CONFIRMED";
            bool isOverdueOrCanceled = eventName == "PAYMENT_OVERDUE" || eventName == "PAYMENT_DELETED" || eventName == "PAYMENT_REFUNDED";

            if (isPaidEvent)
            {
                await _psicologoService.AtualizarStatusPagamentoAsync(subscriptionId, true);
            }
            else if (isOverdueOrCanceled)
            {
                await _psicologoService.AtualizarStatusPagamentoAsync(subscriptionId, false);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar webhook do Asaas: {ex.Message}");
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
