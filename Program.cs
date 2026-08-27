using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Criando a política com nome explícito
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Aplicando a política logo após o Build
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/produtos", async (Produto novoProduto, AppDbContext banco) =>
{
    banco.Produtos.Add(novoProduto);
    await banco.SaveChangesAsync();
    return Results.Created($"/produtos/{novoProduto.Id}", novoProduto);
}).RequireCors("AllowAll");

app.MapGet("/produtos", async (AppDbContext banco) =>
{
    var produtos = await banco.Produtos.ToListAsync();
    return Results.Ok(produtos);
}).RequireCors("AllowAll");

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");