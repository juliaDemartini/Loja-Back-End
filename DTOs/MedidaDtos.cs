using System.ComponentModel.DataAnnotations;

namespace LojaBackEnd.DTOs;

public class MedidaItemDto
{
    [Required(ErrorMessage = "O tamanho é obrigatório (ex: PP, P, M, G, GG).")]
    [MaxLength(10)]
    public string Tamanho { get; set; } = string.Empty;

    public decimal? BustoMin { get; set; }
    public decimal? BustoMax { get; set; }
    public decimal? CinturaMin { get; set; }
    public decimal? CinturaMax { get; set; }
    public decimal? QuadrilMin { get; set; }
    public decimal? QuadrilMax { get; set; }
    public decimal? Comprimento { get; set; }
}

public class TabelaMedidasResponseDto
{
    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public List<MedidaItemDto> Medidas { get; set; } = new();
}