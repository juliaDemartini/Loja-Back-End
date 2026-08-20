using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Models;

namespace LojaBackEnd.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Aqui estamos dizendo: "Crie uma tabela chamada Produtos baseada no molde Produto"
    public DbSet<Produto> Produtos { get; set; }
}