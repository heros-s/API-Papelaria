// FuncionarioController.cs

using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FuncionarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public FuncionarioController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("listar")]
    public async Task<ActionResult<IEnumerable<Funcionario>>> ListarFuncionarios()
    {
        return await _context.Funcionarios.ToListAsync();
    }

    [HttpGet("buscar/{id}")]
    public async Task<ActionResult<Funcionario>> BuscarFuncionarioPorId(int id)
    {
        var funcionario = await _context.Funcionarios.FindAsync(id);

        if (funcionario == null)
            return NotFound();

        return funcionario;
    }

    [HttpPost("cadastrar")]
    public async Task<ActionResult<Funcionario>> CadastrarFuncionario(Funcionario funcionario)
    {
        _context.Funcionarios.Add(funcionario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarFuncionarioPorId), new { id = funcionario.Id }, funcionario);
    }

    [HttpPut("alterar/{id}")]
    public async Task<IActionResult> AlterarFuncionario(int id, Funcionario funcionario)
    {
        if (id != funcionario.Id)
            return BadRequest();

        _context.Entry(funcionario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("remover/{id}")]
    public async Task<IActionResult> RemoverFuncionario(int id)
    {
        var funcionario = await _context.Funcionarios.FindAsync(id);

        if (funcionario == null)
            return NotFound();

        _context.Funcionarios.Remove(funcionario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
