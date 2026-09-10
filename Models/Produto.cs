namespace LojaBackEnd.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public List<MedidaProduto> Medidas { get; set; } = new();

    // Atributos de Filtro (Issue 16)
    public string? Categoria { get; set; }
    public string? Tipo { get; set; }
    public string? Estilo { get; set; }
    public string? Lavagem { get; set; }
    public bool Provador3dDisponivel { get; set; } = false;
}