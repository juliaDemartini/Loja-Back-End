using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do serviço de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Conexão com o PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Criação do banco protegida contra queda de inicialização
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        Console.WriteLine("Banco de dados sincronizado com sucesso.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Aviso ao conectar no banco durante a inicializacao: {ex.Message}");
}

// 3. Middleware de CORS
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Rota para CADASTRAR um novo produto
// Rota para CADASTRAR um novo produto
app.MapPost("/produtos", async (Produto novoProduto, AppDbContext banco) =>
{
    try
    {
        banco.Produtos.Add(novoProduto);
        await banco.SaveChangesAsync();
        return Results.Created($"/produtos/{novoProduto.Id}", novoProduto);
    }
    catch (Exception ex)
    {
        var erroReal = ex.InnerException?.Message ?? ex.Message;
        Console.WriteLine($"ERRO AO SALVAR NO BANCO: {erroReal}");
        return Results.Problem(erroReal, statusCode: 500);
    }
}).RequireCors("AllowAll");

// Rota para LISTAR todos os produtos cadastrados
app.MapGet("/produtos", async (AppDbContext banco) =>
{
    var produtos = await banco.Produtos.ToListAsync();
    return Results.Ok(produtos);
}).RequireCors("AllowAll");

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");