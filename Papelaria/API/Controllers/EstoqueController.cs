// EstoqueController.cs

using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstoqueController : ControllerBase
{
    private readonly AppDbContext _context;
    public EstoqueController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("listar")]
    public async Task<ActionResult<IEnumerable<Estoque>>> ListarEstoques()
    {
        return await _context.Estoques.Include(e => e.Material).ToListAsync();
    }

    [HttpGet("buscar/{id}")]
    public async Task<ActionResult<Estoque>> BuscarEstoquePorId(int id)
    {
        var estoque = await _context.Estoques
            .Include(e => e.Material)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (estoque == null)
            return NotFound();

        return estoque;
    }

    [HttpPost("cadastrar")]
    public async Task<ActionResult<Estoque>> CadastrarEstoque(Estoque estoque)
    {
        // Verifica se o material existe
        var material = await _context.Materiais.FindAsync(estoque.Id);
        if (material == null)
            return NotFound($"Material com ID {estoque.Id} não encontrado.");

        // Verifica se o estoque já existe
        var estoqueExistente = await _context.Estoques.FindAsync(estoque.Id);
        if (estoqueExistente != null)
            return Conflict($"Já existe um estoque registrado para o material com ID {estoque.Id}.");

        // Atribui o material explicitamente (opcional)
        estoque.Material = material;

        _context.Estoques.Add(estoque);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarEstoquePorId), new { id = estoque.Id }, estoque);
    }


    [HttpPut("atualizar/{id}")]
    public async Task<IActionResult> AtualizarEstoque(int id, Estoque estoque)
    {
        if (id != estoque.Id)
            return BadRequest();
        var estoqueExistente = await _context.Estoques.FindAsync(id);
        if (estoqueExistente == null)
            return NotFound();

        estoqueExistente.Quantidade = estoque.Quantidade;
        estoqueExistente.UltimaAtualizacao = DateTime.Now;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("remover/{id}")]
    public async Task<IActionResult> RemoverEstoque(int id)
    {
        var estoque = await _context.Estoques.FindAsync(id);

        if (estoque == null)
            return NotFound();

        _context.Estoques.Remove(estoque);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}