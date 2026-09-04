using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaBackEnd.Models;

public class Pedido
{
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Valores Financeiros
    [Column(TypeName = "decimal(10,2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Frete { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Desconto { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    // Dados de Entrega
    [Required]
    [MaxLength(10)]
    public string Cep { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Logradouro { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Numero { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Complemento { get; set; }

    [Required]
    [MaxLength(80)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    public string Uf { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string TipoFrete { get; set; } = "economico";

    // Pagamento e Status
    [Required]
    [MaxLength(20)]
    public string FormaPagamento { get; set; } = "PIX";

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = "Pendente";

    // Relacionamento com os itens
    public List<ItemPedido> Itens { get; set; } = new();
}