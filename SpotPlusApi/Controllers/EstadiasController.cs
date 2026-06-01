using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotPlusApi.Data;
using SpotPlusApi.Models;

namespace SpotPlusApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstadiasController : ControllerBase
{
    private readonly AppDbContext _context;

    public EstadiasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("entrada")]
public async Task<IActionResult> Entrada(string placa, int estacionamentoId)
{
    var veiculo = await _context.Veiculos
        .FirstOrDefaultAsync(v => v.Placa == placa);

    if (veiculo == null)
        return NotFound("Veículo não encontrado.");

    var estadiaAtiva = await _context.Estadias
        .AnyAsync(e =>
            e.VeiculoId == veiculo.Id &&
            e.Status == "Ativo");

    if (estadiaAtiva)
        return BadRequest("Veículo já possui estadia ativa.");

    var estacionamento = await _context.Estacionamentos
        .FindAsync(estacionamentoId);

    if (estacionamento == null)
        return NotFound("Estacionamento não encontrado.");

    if (veiculo.Tipo == "Carro")
    {
        if (estacionamento.CarrosAtivos >= estacionamento.LimiteCarros)
            return BadRequest("Limite de vagas para carros atingido.");

        estacionamento.CarrosAtivos++;
    }

    if (veiculo.Tipo == "Moto")
    {
        if (estacionamento.MotosAtivas >= estacionamento.LimiteMotos)
            return BadRequest("Limite de vagas para motos atingido.");

        estacionamento.MotosAtivas++;
    }

    var estadia = new Estadia
    {
        VeiculoId = veiculo.Id,
        EstacionamentoId = estacionamentoId,
        DataEntrada = DateTime.Now,
        Status = "Ativo"
    };

    _context.Estadias.Add(estadia);

    await _context.SaveChangesAsync();

    return Ok(estadia);
}

    [HttpPost("saida")]
public async Task<IActionResult> Saida(string placa)
{
    var veiculo = await _context.Veiculos
        .FirstOrDefaultAsync(v => v.Placa == placa);

    if (veiculo == null)
        return NotFound("Veículo não encontrado.");

    var estadia = await _context.Estadias
        .FirstOrDefaultAsync(e =>
            e.VeiculoId == veiculo.Id &&
            e.Status == "Ativo");

    if (estadia == null)
        return NotFound("Estadia não encontrada.");

    estadia.DataSaida = DateTime.Now;

    estadia.TempoMinutos =
        (int)(estadia.DataSaida.Value - estadia.DataEntrada)
        .TotalMinutes;

    var periodos =
        (int)Math.Ceiling(estadia.TempoMinutos / 15.0);

    estadia.ValorTotal = periodos * 3.5m;

    estadia.Status = "Finalizado";

    var estacionamento = await _context.Estacionamentos
        .FindAsync(estadia.EstacionamentoId);

    if (veiculo.Tipo == "Carro")
        estacionamento!.CarrosAtivos--;

    if (veiculo.Tipo == "Moto")
        estacionamento!.MotosAtivas--;

    await _context.SaveChangesAsync();

    return Ok(estadia);
}

    [HttpPatch("{id}")]
    public async Task<IActionResult> AtualizarStatus(int id, string status)
    {
        var estadia = await _context.Estadias.FindAsync(id);

        if (estadia == null)
            return NotFound();

        if (estadia.Status == "Finalizado" &&
            status == "Ativo")
        {
            return Conflict(
                "Uma estadia finalizada não pode ser reaberta.");
        }

        estadia.Status = status;

        await _context.SaveChangesAsync();

        return Ok(estadia);
    }
}