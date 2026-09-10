using System.Text.Json.Serialization;
namespace LojaBackEnd.Models;

public class ImagemProduto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool Principal { get; set; } = false;
    public string? CorHexVinculada { get; set; }

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!; // <--- Adicione esta linha
}