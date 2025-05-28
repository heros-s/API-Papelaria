// ContaPagar.cs

namespace API.Models;

public class ContaPagar
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public string Status { get; set; } = string.Empty;
}