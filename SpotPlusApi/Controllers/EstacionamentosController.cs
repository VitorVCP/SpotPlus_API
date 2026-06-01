using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotPlusApi.Data;
using SpotPlusApi.Models;

namespace SpotPlusApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstacionamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public EstacionamentosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estacionamento>>> GetEstacionamentos()
    {
        return await _context.Estacionamentos.ToListAsync();
    }

    [HttpGet("estacionados/{estacionamentoId}")]
    public async Task<IActionResult> VeiculosEstacionados(int estacionamentoId)
    {
        var estadiasAtivas = await _context.Estadias
            .Where(e =>
                e.EstacionamentoId == estacionamentoId &&
                e.Status == "Ativo")
            .ToListAsync();

        var veiculos = new List<object>();

        foreach (var estadia in estadiasAtivas)
        {
            var veiculo = await _context.Veiculos
                .FindAsync(estadia.VeiculoId);

            veiculos.Add(new
            {
                veiculo!.Placa,
                veiculo.Tipo,
                estadia.DataEntrada
            });
        }

        return Ok(veiculos);
    }

    [HttpPost]
    public async Task<ActionResult<Estacionamento>> PostEstacionamento(Estacionamento estacionamento)
    {
        _context.Estacionamentos.Add(estacionamento);

        await _context.SaveChangesAsync();

        return Ok(estacionamento);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEstacionamento(int id)
    {
        var estacionamento = await _context.Estacionamentos.FindAsync(id);

        if (estacionamento == null)
            return NotFound("Estacionamento não encontrado");

        _context.Estacionamentos.Remove(estacionamento);

        await _context.SaveChangesAsync();

        return Ok("Estacionamento removido com sucesso!");
    }
}