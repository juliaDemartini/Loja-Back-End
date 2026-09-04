using System.ComponentModel.DataAnnotations;

namespace LojaBackEnd.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string SenhaHash { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefone { get; set; }

    [MaxLength(14)]
    public string? Cpf { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}