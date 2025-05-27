// Funcionario.cs
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Funcionario
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string Cargo { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Salario { get; set; }

    public DateTime DataContratacao { get; set; }
}