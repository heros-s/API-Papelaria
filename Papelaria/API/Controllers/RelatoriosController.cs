using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models; // Add this line if StatusVenda is defined in API.Models namespace


namespace PapelariaEmpresarial.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatoriosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("contabilidade")]
        public async Task<IActionResult> GetRelatorioContabilidade()
        {
            // Total de receitas: somando itens vendidos
            var totalReceitas = await _context.Vendas
                .Include(v => v.Itens)
                .SumAsync(v => v.Itens.Sum(i => i.PrecoUnitario * i.Quantidade));

            // Total de despesas: somando salários dos funcionários
            var totalDespesas = await _context.Funcionarios
                .SumAsync(f => f.Salario);

            var resultado = new
            {
                Receitas = totalReceitas,
                Despesas = totalDespesas,
                Lucro = totalReceitas - totalDespesas
            };

            return Ok(resultado);
        }
    }
}
