using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendaController : ControllerBase
{
    private readonly AppDbContext _context;

    public VendaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("efetuar/{carrinhoId}")]
    public async Task<ActionResult<Venda>> EfetuarVenda(int carrinhoId)
    {
        var carrinho = await _context.Carrinhos
            .Include(c => c.Itens)
            .ThenInclude(i => i.Material)
            .FirstOrDefaultAsync(c => c.Id == carrinhoId);

        if (carrinho == null || carrinho.Itens.Count == 0)
            return BadRequest("Carrinho não encontrado ou vazio.");

        foreach (var item in carrinho.Itens)
        {
            var estoque = await _context.Estoques.FindAsync(item.MaterialId);
            if (estoque == null || estoque.Quantidade < item.Quantidade)
                return BadRequest($"Estoque insuficiente para o material: {item.Material?.Nome ?? item.MaterialId.ToString()}");
        }

        var venda = new Venda
        {
            Total = carrinho.Total,
            Itens = carrinho.Itens.Select(i => new ItemVenda
            {
                MaterialId = i.MaterialId,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };

        _context.Vendas.Add(venda);

        foreach (var item in carrinho.Itens)
        {
            var estoque = await _context.Estoques.FindAsync(item.MaterialId);
            estoque!.Quantidade -= item.Quantidade;
            estoque.UltimaAtualizacao = DateTime.Now;
        }
        _context.ItensCarrinho.RemoveRange(carrinho.Itens);
        _context.Carrinhos.Remove(carrinho);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarVendaPorId), new { id = venda.Id }, venda);
    }

    [HttpGet("listar")]
    public async Task<ActionResult<IEnumerable<Venda>>> ListarVendas()
    {
        return await _context.Vendas
            .Include(v => v.Itens)
            .ThenInclude(i => i.Material)
            .ToListAsync();
    }

    [HttpGet("buscar/{id}")]
    public async Task<ActionResult<Venda>> BuscarVendaPorId(int id)
    {
        var venda = await _context.Vendas
            .Include(v => v.Itens)
            .ThenInclude(i => i.Material)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venda == null)
            return NotFound();

        return venda;
    }
}
