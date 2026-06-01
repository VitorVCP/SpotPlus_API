using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotPlusApi.Data;
using SpotPlusApi.Models;

namespace SpotPlusApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VeiculosController : ControllerBase
{
    private readonly AppDbContext _context;

    public VeiculosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
    {
        return await _context.Veiculos.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
    {
        if (string.IsNullOrWhiteSpace(veiculo.Placa))
            return BadRequest("Placa inválida.");

        _context.Veiculos.Add(veiculo);

        await _context.SaveChangesAsync();

        return Ok(veiculo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVeiculo(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);

        if (veiculo == null)
            return NotFound("Veiculo não encontrado");

        _context.Veiculos.Remove(veiculo);

        await _context.SaveChangesAsync();

        return Ok("Veiculo removido com sucesso!");
    }
}