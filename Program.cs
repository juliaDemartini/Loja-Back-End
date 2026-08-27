using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do serviço de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Conectando no PostgreSQL usando a URL que está no appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Habilitação do middleware de CORS (deve vir antes das rotas)
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Rota para CADASTRAR um novo produto
app.MapPost("/produtos", async (Produto novoProduto, AppDbContext banco) =>
{
    banco.Produtos.Add(novoProduto);
    await banco.SaveChangesAsync();
    return Results.Created($"/produtos/{novoProduto.Id}", novoProduto);
});

// Rota para LISTAR todos os produtos cadastrados
app.MapGet("/produtos", async (AppDbContext banco) =>
{
    var produtos = await banco.Produtos.ToListAsync();
    return Results.Ok(produtos);
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");