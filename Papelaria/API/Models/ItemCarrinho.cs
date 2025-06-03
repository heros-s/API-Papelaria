// ItemCarrinho.cs

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using API.Models;

public class ItemCarrinho
{
    public int Id { get; set; }

    public int CarrinhoId { get; set; }
    [JsonIgnore]
    public Carrinho? Carrinho { get; set; }

    public int MaterialId { get; set; }
    public Material? Material { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}