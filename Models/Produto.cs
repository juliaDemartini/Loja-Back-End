namespace LojaBackEnd.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }

    // Ficha Técnica (Novos campos)
    public string? Descricao { get; set; }
    public string? Composicao { get; set; }
    public string? Modelagem { get; set; }
    public string? Elasticidade { get; set; }
    public string? Cuidados { get; set; }

    // Relações de Galeria e Cores
    public List<ImagemProduto> Imagens { get; set; } = new();
    public List<CorProduto> Cores { get; set; } = new();
    public List<MedidaProduto> Medidas { get; set; } = new();

    // Filtros anteriores
    public string? Categoria { get; set; }
    public string? Tipo { get; set; }
    public string? Estilo { get; set; }
    public string? Lavagem { get; set; }
    public bool Provador3dDisponivel { get; set; } = false;
}