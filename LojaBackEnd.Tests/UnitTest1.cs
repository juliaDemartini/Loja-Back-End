
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using LojaBackEnd.Data;
using LojaBackEnd.Models;
using LojaBackEnd.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LojaBackEnd.Tests;

public class UnitTest1 : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public UnitTest1(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    // TESTE 1 - CINDY
    [Fact]
    public void Teste4_StatusPedidoValido_DeveSerAceito()
    {
        var statusPermitidos = new[] { "Pendente", "Pago", "Enviado", "Entregue" };
        var novoStatus = "Pago";

        Assert.Contains(novoStatus, statusPermitidos);
    }

    // TESTE 2 - CINDY
    [Fact]
    public void Usuario_NomeMuitoLongo_DeveFalharNaValidacao()
    {
        var usuarioInvalido = new Usuario
        {
            Nome = new string('A', 105),
            Email = "teste@exemplo.com",
            SenhaHash = "hash_seguro_123"
        };

        var contexto = new ValidationContext(usuarioInvalido);
        var resultados = new List<ValidationResult>();

        var ehValido = Validator.TryValidateObject(
            usuarioInvalido,
            contexto,
            resultados,
            validateAllProperties: true
        );

        Assert.False(ehValido);
    }

    // TESTE 3 - JULIA
    [Fact]
    public async Task GetProdutos_DeveRetornarListaDeProdutos()
    {
        using var scope = _factory.Services.CreateScope();

        var banco = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        banco.Produtos.Add(new Produto
        {
            Nome = "Calça Jeans Teste",
            Preco = 99.90m,
            Categoria = "Calças"
        });

        await banco.SaveChangesAsync();

        var resposta = await _client.GetAsync("/api/produtos");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var conteudo = await resposta.Content.ReadAsStringAsync();

        using var json = JsonDocument.Parse(conteudo);

        Assert.Equal(JsonValueKind.Array, json.RootElement.ValueKind);

        Assert.Contains("Calça Jeans Teste", conteudo);
    }

    // TESTE 4 - JULIA
    [Fact]
    public async Task PostProdutos_DeveCadastrarProdutoComSucesso()
    {
        var produto = new
        {
            Nome = "Calça Jeans Cadastro Teste",
            Preco = 149.90m,
            Categoria = "Calças",
            Tipo = "Jeans",
            Estilo = "Casual"
        };

        var json = JsonSerializer.Serialize(produto);

        var conteudo = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var resposta = await _client.PostAsync("/api/produtos", conteudo);

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

        var respostaJson = await resposta.Content.ReadAsStringAsync();

        Assert.Contains("Calça Jeans Cadastro Teste", respostaJson);
    }
}
