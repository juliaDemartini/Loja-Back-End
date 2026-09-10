using System.Text.Json.Serialization;

public class ImagemProduto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool Principal { get; set; }
    public string? CorHexVinculada { get; set; }

    
    [JsonIgnore]
    public Produto? Produto { get; set; }
}