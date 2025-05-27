// CarrinhoController.cs

using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrinhoController : ControllerBase
{
    private readonly AppDbContext _context;
    public CarrinhoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("criar")]
    public async Task<ActionResult<Carrinho>> CriarCarrinho()
    {
        var carrinho = new Carrinho();
        _context.Carrinhos.Add(carrinho);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(BuscarCarrinhoPorId), new { id = carrinho.Id }, carrinho);
    }

    [HttpPost("adicionar-item")]
    public async Task<ActionResult> AdicionarItem(ItemCarrinho item)
    {
        var carrinho = await _context.Carrinhos
        .Include(c => c.Itens)
        .FirstOrDefaultAsync(c => c.Id == item.CarrinhoId);

        if (carrinho == null)
        {
            return NotFound("Carrinho não encontrado.");
        }

        var material = await _context.Materiais.FindAsync(item.MaterialId);
        if (material == null)
        {
            return NotFound("Material não encontrado.");
        }

        var estoque = await _context.Estoques.FindAsync(item.MaterialId);
        if (estoque == null || estoque.Quantidade < item.Quantidade)
        {
            return BadRequest("Estoque insuficiente para o material solicitado.");
        }

        var itemExistente = carrinho.Itens.FirstOrDefault(i => i.MaterialId == item.MaterialId);

        if (itemExistente != null)
        {
            var novaQuantidade = itemExistente.Quantidade + item.Quantidade;

            if (estoque.Quantidade < novaQuantidade)
            {
                return BadRequest("Estoque insuficiente ao somar com o item já existente no carrinho.");
            }

            itemExistente.Quantidade = novaQuantidade;
        }
        else
        {
            item.PrecoUnitario = material.Preco;
            carrinho.Itens.Add(item);
        }

        await _context.SaveChangesAsync();
        return Ok("Item adicionado ao carrinho.");
    }


    [HttpDelete("remover-item/{itemId}")]
    public async Task<IActionResult> RemoverItem(int itemId)
    {
        var item = await _context.ItensCarrinho.FindAsync(itemId);
        if (item == null)
            return NotFound();

        _context.ItensCarrinho.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("buscar/{id}")]
    public async Task<ActionResult<Carrinho>> BuscarCarrinhoPorId(int id)
    {
        var carrinho = await _context.Carrinhos
            .Include(c => c.Itens)
            .ThenInclude(i => i.Material)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (carrinho == null)
            return NotFound();

        return carrinho;
    }

    [HttpDelete("limpar/{id}")]
    public async Task<IActionResult> LimparCarrinho(int id)
    {
        var itens = _context.ItensCarrinho.Where(i => i.CarrinhoId == id);
        _context.ItensCarrinho.RemoveRange(itens);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("deletar/{id}")]
    public async Task<IActionResult> DeletarCarrinho(int id)
    {
        var carrinho = await _context.Carrinhos.FindAsync(id);
        if (carrinho == null)
            return NotFound();

        var itens = _context.ItensCarrinho.Where(i => i.CarrinhoId == id);
        _context.ItensCarrinho.RemoveRange(itens);
        _context.Carrinhos.Remove(carrinho);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
