namespace LojaBackEnd.DTOs;

public class ProdutoRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public string? Categoria { get; set; }
    public string? Tipo { get; set; }
    public string? Estilo { get; set; }
    public string? Lavagem { get; set; }
    public bool Provador3dDisponivel { get; set; }
}

public class ProdutoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public string? Categoria { get; set; }
    public string? Tipo { get; set; }
    public string? Estilo { get; set; }
    public string? Lavagem { get; set; }
    public bool Provador3dDisponivel { get; set; }
}