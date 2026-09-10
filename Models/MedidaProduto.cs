using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LojaBackEnd.Models;

public class MedidaProduto
{
    public int Id { get; set; }

    [Required]
    public int ProdutoId { get; set; }

   public string Tamanho { get; set; } = string.Empty; // PP, P, M, G, GG

    [Column(TypeName = "decimal(6,2)")]
    public decimal? BustoMin { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal? BustoMax { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal? CinturaMin { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal? CinturaMax { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal? QuadrilMin { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal? QuadrilMax { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal? Comprimento { get; set; }

     [JsonIgnore]
        public Produto? Produto { get; set; }
}