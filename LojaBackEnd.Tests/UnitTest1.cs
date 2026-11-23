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

    [Fact]
    public async Task GetProdutos_DeveRetornarListaDeProdutos()
    {
        // Arrange: cadastrar um produto fictício no banco de testes
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

        // Act: solicitar a listagem de produtos
        var resposta = await _client.GetAsync("/api/produtos");

        // Assert: verificar a resposta
        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var conteudo = await resposta.Content.ReadAsStringAsync();

        using var json = JsonDocument.Parse(conteudo);

        Assert.Equal(
            JsonValueKind.Array,
            json.RootElement.ValueKind
        );

        Assert.Contains("Calça Jeans Teste", conteudo);
    }

[Fact]
public async Task PostProdutos_DeveCadastrarProdutoComSucesso()
{
    // Arrange
    var produto = new
    {
        Nome = "Calça Jeans Cadastro Teste",
        Preco = 149.90m,
        Categoria = "Calças",
        Tipo = "Jeans",
        Estilo = "Casual"
    };

    var json = System.Text.Json.JsonSerializer.Serialize(produto);

    var conteudo = new StringContent(
        json,
        System.Text.Encoding.UTF8,
        "application/json"
    );

    // Act
    var resposta = await _client.PostAsync("/api/produtos", conteudo);

    // Assert
    Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

    var respostaJson = await resposta.Content.ReadAsStringAsync();

    Assert.Contains("Calça Jeans Cadastro Teste", respostaJson);
}
}