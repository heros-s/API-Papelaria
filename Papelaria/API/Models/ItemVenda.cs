// ItemVenda.cs

using System.Text.Json.Serialization;

namespace API.Models;

public class ItemVenda
{
    public int Id { get; set; }

    public int VendaId { get; set; }
    [JsonIgnore]
    public Venda? Venda { get; set; }

    public int MaterialId { get; set; }
    public Material? Material { get; set; }

    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}