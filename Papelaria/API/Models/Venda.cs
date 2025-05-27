namespace API.Models;

public class Venda
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.Now;
    public decimal Total { get; set; }
    public List<ItemVenda> Itens { get; set; } = new();
}