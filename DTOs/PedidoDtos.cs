using System.ComponentModel.DataAnnotations;

namespace LojaBackEnd.DTOs;

public class ClienteCheckoutDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }
    public string? Cpf { get; set; }
}

public class EnderecoCheckoutDto
{
    [Required(ErrorMessage = "O CEP é obrigatório.")]
    public string Cep { get; set; } = string.Empty;

    [Required(ErrorMessage = "O logradouro é obrigatório.")]
    public string Logradouro { get; set; } = string.Empty;

    [Required(ErrorMessage = "O número é obrigatório.")]
    public string Numero { get; set; } = string.Empty;

    public string? Complemento { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório.")]
    public string Bairro { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "A UF é obrigatória.")]
    public string Uf { get; set; } = string.Empty;
}

public class ItemCheckoutDto
{
    [Required]
    public int ProdutoId { get; set; }

    public string Nome { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser de pelo menos 1.")]
    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public string Tamanho { get; set; } = "M";

    public string? ImagemUrl { get; set; }
}

public class CheckoutRequestDto
{
    [Required]
    public ClienteCheckoutDto Cliente { get; set; } = new();

    [Required]
    public EnderecoCheckoutDto Endereco { get; set; } = new();

    [Required]
    [MinLength(1, ErrorMessage = "O pedido precisa conter pelo menos um item.")]
    public List<ItemCheckoutDto> Itens { get; set; } = new();

    public string TipoFrete { get; set; } = "economico";
    public decimal ValorFrete { get; set; }
    public string FormaPagamento { get; set; } = "PIX";
    public decimal Subtotal { get; set; }
    public decimal DescontoPix { get; set; }
    public decimal TotalFinal { get; set; }
}

public class PedidoCriadoRespostaDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalFinal { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ValorFrete { get; set; }
    public decimal DescontoPix { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
    public string TipoFrete { get; set; } = string.Empty;
    public ClienteCheckoutDto Cliente { get; set; } = new();
    public EnderecoCheckoutDto Endereco { get; set; } = new();
    public List<ItemCheckoutDto> Itens { get; set; } = new();
}

public class ItemPedidoHistoricoDto
{
    public int ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string Tamanho { get; set; } = string.Empty;
    public string? ImagemUrl { get; set; }
}

public class PedidoHistoricoDto
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Frete { get; set; }
    public decimal Desconto { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
    public string TipoFrete { get; set; } = string.Empty;
    public List<ItemPedidoHistoricoDto> Itens { get; set; } = new();
}

public class AtualizarStatusPedidoDto
{
    [Required(ErrorMessage = "O status é obrigatório.")]
    public string Status { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "O código de rastreio deve ter no máximo 50 caracteres.")]
    public string? CodigoRastreio { get; set; }
}