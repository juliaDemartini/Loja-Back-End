namespace LojaBackEnd.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public List<MedidaProduto> Medidas { get; set; } = new();
}