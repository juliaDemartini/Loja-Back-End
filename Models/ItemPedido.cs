using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaBackEnd.Models;

public class ItemPedido
{
    public int Id { get; set; }

    [Required]
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    [Required]
    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    [Required]
    public int Quantidade { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoUnitario { get; set; }

    [Required]
    [MaxLength(10)]
    public string Tamanho { get; set; } = "M";
}