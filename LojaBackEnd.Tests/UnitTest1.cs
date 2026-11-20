using System.ComponentModel.DataAnnotations;
using LojaBackEnd.Models;

namespace LojaBackEnd.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {

    }
       //Teste de validação de status - Cindy //
    [Fact]
    public void Teste4_StatusPedidoValido_DeveSerAceito()
    {
        var statusPermitidos = new[] { "Pendente", "Pago", "Enviado", "Entregue" };
        var novoStatus = "Pago";

        Assert.Contains(novoStatus, statusPermitidos);
    }

// Teste de limite do Nome no modelo Usuario - Cindy
    [Fact]
    public void Usuario_NomeDentroDoLimite100_DeveSerValido()
    {
        var usuario = new Usuario
        {
            Nome = new string('A', 100),
            Email = "cindyane@exemplo.com",
            SenhaHash = "hash_seguro_123"
        };

        var contexto = new ValidationContext(usuario);
        var resultados = new List<ValidationResult>();
        var valido = Validator.TryValidateObject(usuario, contexto, resultados, validateAllProperties: true);

        Assert.True(valido);
        Assert.Empty(resultados);
    }
}