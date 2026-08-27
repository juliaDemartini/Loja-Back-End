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

// Conectando no PostgreSQL usando a URL do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Criação automática da tabela no Supabase caso ela ainda não exista
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// 3. Middleware de CORS
app.UseCors("AllowAll");

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
}).RequireCors("AllowAll");

// Rota para LISTAR todos os produtos cadastrados
app.MapGet("/produtos", async (AppDbContext banco) =>
{
    var produtos = await banco.Produtos.ToListAsync();
    return Results.Ok(produtos);
}).RequireCors("AllowAll");

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");