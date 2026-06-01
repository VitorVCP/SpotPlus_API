using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotPlusApi.Data;

namespace SpotPlusApi.Controllers;

[Route("api [controller]")]
[ApiController]
public class FinanceiroController : ControllerBase
{
    private readonly AppDbContext _context;

    public FinanceiroController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("faturamento")]
    public async Task<IActionResult> Faturamento()
    {
        await Task.Delay(3000);

        var total = await _context.Pagamentos
            .SumAsync(p => p.ValorPago);

        var quantidadeVeiculos =
            await _context.Veiculos.CountAsync();

        var quantidadeCarros =
            await _context.Veiculos
                .CountAsync(v => v.Tipo == "Carro");

        var quantidadeMotos =
            await _context.Veiculos
                .CountAsync(v => v.Tipo == "Moto");

        var formaMaisUsada = await _context.Pagamentos
            .GroupBy(p => p.FormaPagamento)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync();

        return Ok(new
        {
            totalArrecadado = total,
            quantidadeVeiculos,
            quantidadeCarros,
            quantidadeMotos,
            formaMaisUsada
        });
    }
}