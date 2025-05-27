// MaterialController.cs

using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly AppDbContext _context;

    public MaterialController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("listar")]
    public async Task<ActionResult<IEnumerable<Material>>> ListarMateriais()
    {
        return await _context.Materiais.ToListAsync();
    }

    [HttpGet("buscar/{id}")]
    public async Task<ActionResult<Material>> BuscarMaterialPorId(int id)
    {
        var material = await _context.Materiais.FindAsync(id);

        if (material == null)
            return NotFound();

        return material;
    }

    [HttpPost("cadastrar")]
    public async Task<ActionResult<Material>> CadastrarMaterial(Material material)
    {
        _context.Materiais.Add(material);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarMaterialPorId), new { id = material.Id }, material);
    }

    [HttpPut("alterar/{id}")]
    public async Task<IActionResult> AlterarMaterial(int id, Material material)
    {
        if (id != material.Id)
            return BadRequest();

        _context.Entry(material).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("remover/{id}")]
    public async Task<IActionResult> RemoverMaterial(int id)
    {
        var material = await _context.Materiais.FindAsync(id);

        if (material == null)
            return NotFound();

        var estoque = await _context.Estoques.FindAsync(id);
        if (estoque != null)
            return Conflict("Não é possível remover um material que possui estoque registrado.");

        _context.Materiais.Remove(material);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
