using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotPlusApi.Data;
using SpotPlusApi.Models;

namespace SpotPlusApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PagamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public PagamentosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("pendentes/{clienteId}")]
    public async Task<IActionResult> PagamentosPendentes(int clienteId)
    {
        var cliente = await _context.Clientes
            .FindAsync(clienteId);

        if (cliente == null)
            return NotFound("Cliente não encontrado.");

        var veiculos = await _context.Veiculos
            .Where(v => v.ClienteId == clienteId)
            .ToListAsync();

        var resultado = new List<object>();

        foreach (var veiculo in veiculos)
        {
            var estadias = await _context.Estadias
                .Where(e =>
                    e.VeiculoId == veiculo.Id &&
                    e.Status == "Finalizado")
                .ToListAsync();

            foreach (var estadia in estadias)
            {
                var pagamentoExiste = await _context.Pagamentos
                    .AnyAsync(p => p.EstadiaId == estadia.Id);

                if (!pagamentoExiste)
                {
                    resultado.Add(new
                    {
                        EstadiaId = estadia.Id,
                        veiculo.Placa,
                        veiculo.Tipo,
                        Valor = estadia.ValorTotal,
                        Entrada = estadia.DataEntrada,
                        Saida = estadia.DataSaida
                    });
                }
            }
        }

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Pagar(int estadiaId, string formaPagamento)
    {
        var estadia = await _context.Estadias
            .FindAsync(estadiaId);

        if (estadia == null)
            return BadRequest("Estadia inválida.");

        var pagamentoExistente = await _context.Pagamentos
            .AnyAsync(p => p.EstadiaId == estadiaId);

        if (pagamentoExistente)
            return BadRequest("Esta estadia já foi paga.");

        var veiculo = await _context.Veiculos
            .FindAsync(estadia.VeiculoId);

        var cliente = await _context.Clientes
            .FindAsync(veiculo!.ClienteId);

        if (formaPagamento == "Pix")
        {
            if (cliente!.SaldoPix < estadia.ValorTotal)
                return BadRequest("Saldo Pix insuficiente.");

            cliente.SaldoPix -= estadia.ValorTotal;
        }

        else if (formaPagamento == "Debito")
        {
            if (cliente!.SaldoDebito < estadia.ValorTotal)
                return BadRequest("Saldo Débito insuficiente.");

            cliente.SaldoDebito -= estadia.ValorTotal;
        }

        else if (formaPagamento == "Credito")
        {
            cliente!.LimiteCredito -= estadia.ValorTotal;
        }

        else
        {
            return BadRequest("Forma de pagamento inválida.");
        }

        var pagamento = new Pagamento
        {
            EstadiaId = estadia.Id,
            FormaPagamento = formaPagamento,
            ValorPago = estadia.ValorTotal,
            Pago = true,
            DataPagamento = DateTime.Now
        };

        _context.Pagamentos.Add(pagamento);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Mensagem = "Pagamento realizado com sucesso.",
            FormaPagamento = formaPagamento,
            ValorPago = estadia.ValorTotal,

            SaldoAtual = new
            {
                cliente.SaldoPix,
                cliente.SaldoDebito,
                cliente.LimiteCredito
            }
        });
    }
}