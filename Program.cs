using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.Models;

var builder = WebApplication.CreateBuilder(args);

// Conectando no PostgreSQL usando a URL que está no appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Rota para CADASTRAR um novo produto
app.MapPost("/produtos", async (Produto novoProduto, AppDbContext banco) =>
{
    // 1. Adiciona a nova roupa na tabela do Entity Framework
    banco.Produtos.Add(novoProduto);
    
    // 2. Salva a alteração de verdade no Supabase
    await banco.SaveChangesAsync();
    
    // 3. Responde com sucesso (Código HTTP 201) e mostra o item salvo
    return Results.Created($"/produtos/{novoProduto.Id}", novoProduto);
});

// Rota para LISTAR todos os produtos cadastrados
app.MapGet("/produtos", async (AppDbContext banco) =>
{
    // Vai no banco, pega todos os produtos e transforma numa lista
    var produtos = await banco.Produtos.ToListAsync();
    
    // Responde com sucesso (Código HTTP 200) entregando a lista
    return Results.Ok(produtos);
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
