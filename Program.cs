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

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Rota POST (Cadastrar)
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
        var detalhe = ex.InnerException?.Message ?? ex.Message;
        Console.WriteLine($"ERRO DB POST: {detalhe}");
        return Results.Problem(detail: detalhe, statusCode: 500);
    }
}).RequireCors("AllowAll");

// Rota GET (Listar)
app.MapGet("/produtos", async (AppDbContext banco) =>
{
    try
    {
        var produtos = await banco.Produtos.ToListAsync();
        return Results.Ok(produtos);
    }
    catch (Exception ex)
    {
        var detalhe = ex.InnerException?.Message ?? ex.Message;
        Console.WriteLine($"ERRO DB GET: {detalhe}");
        return Results.Problem(detail: detalhe, statusCode: 500);
    }
}).RequireCors("AllowAll");

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");