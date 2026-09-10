using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. Conexão com o Banco de Dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Grupo centralizado para todas as rotas começarem com /api
var api = app.MapGroup("/api");

// ==========================================
// ROTAS DE PRODUTOS
// ==========================================

// Rota POST (Cadastrar Produto)
api.MapPost("/produtos", async (Produto novoProduto, AppDbContext banco) =>
{
    try
    {
        var capa = novoProduto.Imagens?.FirstOrDefault(i => i.Principal) ?? novoProduto.Imagens?.FirstOrDefault();
        if (capa != null)
        {
            novoProduto.ImagemUrl = capa.Url;
        }

        banco.Produtos.Add(novoProduto);
        await banco.SaveChangesAsync();
        return Results.Created($"/api/produtos/{novoProduto.Id}", novoProduto);
    }
    catch (Exception ex)
    {
        // Isso vai imprimir o erro exato no log do Render
        var erroCompleto = ex.InnerException?.Message ?? ex.Message;
        Console.WriteLine($"ERRO CRITICO NO POST: {erroCompleto} | STACK: {ex.StackTrace}");
        return Results.Problem(detail: erroCompleto, statusCode: 500);
    }
}).RequireCors("AllowAll");
// Rota GET (Listar Produtos)
api.MapGet("/produtos", async (AppDbContext banco) =>
{
    try
    {
        var produtos = await banco.Produtos
            .Include(p => p.Imagens)
            .Include(p => p.Cores)
            .Include(p => p.Medidas)
            .ToListAsync();
        return Results.Ok(produtos);
    }
    catch (Exception ex)
    {
        var detalhe = ex.InnerException?.Message ?? ex.Message;
        Console.WriteLine($"ERRO DB GET: {detalhe}");
        return Results.Problem(detail: detalhe, statusCode: 500);
    }
}).RequireCors("AllowAll");

// Rota PUT (Atualizar Produto)
api.MapPut("/produtos/{id:int}", async (int id, Produto produtoAtualizado, AppDbContext banco) =>
{
    try
    {
        var produtoExistente = await banco.Produtos
            .Include(p => p.Imagens)
            .Include(p => p.Cores)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (produtoExistente is null)
        {
            return Results.NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });
        }

        produtoExistente.Nome = produtoAtualizado.Nome;
        produtoExistente.Preco = produtoAtualizado.Preco;
        produtoExistente.Descricao = produtoAtualizado.Descricao;
        produtoExistente.Composicao = produtoAtualizado.Composicao;
        produtoExistente.Modelagem = produtoAtualizado.Modelagem;
        produtoExistente.Elasticidade = produtoAtualizado.Elasticidade;
        produtoExistente.Cuidados = produtoAtualizado.Cuidados;

        produtoExistente.Categoria = produtoAtualizado.Categoria;
        produtoExistente.Tipo = produtoAtualizado.Tipo;
        produtoExistente.Estilo = produtoAtualizado.Estilo;
        produtoExistente.Lavagem = produtoAtualizado.Lavagem;
        produtoExistente.Provador3dDisponivel = produtoAtualizado.Provador3dDisponivel;

        banco.ImagensProdutos.RemoveRange(produtoExistente.Imagens);
        produtoExistente.Imagens = produtoAtualizado.Imagens ?? new();

        banco.CoresProdutos.RemoveRange(produtoExistente.Cores);
        produtoExistente.Cores = produtoAtualizado.Cores ?? new();

        var capa = produtoExistente.Imagens.FirstOrDefault(i => i.Principal) ?? produtoExistente.Imagens.FirstOrDefault();
        produtoExistente.ImagemUrl = capa?.Url ?? produtoAtualizado.ImagemUrl;

        await banco.SaveChangesAsync();

        return Results.Ok(produtoExistente);
    }
    catch (Exception ex)
    {
        var detalhe = ex.InnerException?.Message ?? ex.Message;
        Console.WriteLine($"ERRO DB PUT: {detalhe}");
        return Results.Problem(detail: detalhe, statusCode: 500);
    }
}).RequireCors("AllowAll");

// Rota DELETE (Remover Produto)
api.MapDelete("/produtos/{id:int}", async (int id, AppDbContext banco) =>
{
    try
    {
        var produto = await banco.Produtos.FindAsync(id);
        
        if (produto is null)
        {
            return Results.NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });
        }

        banco.Produtos.Remove(produto);
        await banco.SaveChangesAsync();

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        var detalhe = ex.InnerException?.Message ?? ex.Message;
        Console.WriteLine($"ERRO DB DELETE: {detalhe}");
        return Results.Problem(detail: detalhe, statusCode: 500);
    }
}).RequireCors("AllowAll");

// ==========================================
// ROTAS DE PEDIDOS 
// ==========================================

api.MapGet("/pedidos", async (AppDbContext banco) =>
{
    try
    {
        // Retorna lista vazia caso a tabela de pedidos ainda não esteja criada no banco
        return Results.Ok(new List<object>());
    }
    catch (Exception ex)
    {
        var detalhe = ex.InnerException?.Message ?? ex.Message;
        Console.WriteLine($"ERRO DB GET PEDIDOS: {detalhe}");
        return Results.Problem(detail: detalhe, statusCode: 500);
    }
}).RequireCors("AllowAll");

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.MapControllers();
app.Run($"http://0.0.0.0:{port}");