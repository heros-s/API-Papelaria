// Estoque.cs

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Estoque
{
    [Key, ForeignKey("Material")]
    public int Id { get; set; }

    public Material? Material { get; set; }
    [Range(0, int.MaxValue)]
    public int Quantidade { get; set; }
    public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;
    
}