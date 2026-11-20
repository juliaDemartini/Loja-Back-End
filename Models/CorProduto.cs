using System.Text.Json.Serialization;

namespace LojaBackEnd.Models
{
    public class CorProduto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CodigoHex { get; set; } = string.Empty;

        [JsonIgnore]
        public Produto? Produto { get; set; }
    }
}