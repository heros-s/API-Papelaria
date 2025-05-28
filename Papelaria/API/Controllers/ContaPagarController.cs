// ContaPagarController.cs

using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContasPagarController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContasPagarController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("listar")]
    public async Task<ActionResult<IEnumerable<ContaPagar>>> GetContas() =>
        await _context.ContasPagar.ToListAsync();

    [HttpGet("buscar/{id}")]
    public async Task<ActionResult<ContaPagar>> GetConta(int id)
    {
        var conta = await _context.ContasPagar.FindAsync(id);
        return conta == null ? NotFound() : Ok(conta);
    }

    [HttpPost("cadastrar")]
    public async Task<ActionResult<ContaPagar>> PostConta(ContaPagar conta)
    {
        _context.ContasPagar.Add(conta);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetConta), new { id = conta.Id }, conta);
    }

    [HttpPut("atualizar/{id}")]
    public async Task<IActionResult> PutConta(int id, ContaPagar conta)
    {
        if (id != conta.Id) return BadRequest();

        _context.Entry(conta).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("remover/{id}")]
    public async Task<IActionResult> DeleteConta(int id)
    {
        var conta = await _context.ContasPagar.FindAsync(id);
        if (conta == null) return NotFound();

        _context.ContasPagar.Remove(conta);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
