// Carrinho.cs

public class Carrinho
{
    public int Id { get; set; }
    public List<ItemCarrinho> Itens { get; set; } = new();
    public decimal Total => Itens.Sum(i => i.PrecoUnitario * i.Quantidade);
    public string Status { get; set; } = "EmCarrinho";
}
