using System.ComponentModel.DataAnnotations;
using LojaBackEnd.Models;

namespace LojaBackEnd.Tests;

public class UnitTest1
{
       //Teste de validação de status - Cindy //
   [Fact]
    public void Teste4_StatusPedidoValido_DeveSerAceito()
    {
        var statusPermitidos = new[] { "Pendente", "Pago", "Enviado", "Entregue" };
        var novoStatus = "Pago";

        Assert.Contains(novoStatus, statusPermitidos);
    }

    // Teste de falha proposital para demonstração do pipeline
    [Fact]
    public void Usuario_NomeMuitoLongo_DeveFalharNaValidacao()
    {
        // Instanciando o objeto diretamente no teste com um nome inválido
        var usuarioInvalido = new Usuario
        {
            Nome = new string('A', 105), // Nome com 105 caracteres (ultrapassa o limite de 100)
            Email = "teste@exemplo.com",
            SenhaHash = "hash_seguro_123"
        };

        var contexto = new ValidationContext(usuarioInvalido);
        var resultados = new List<ValidationResult>();
        
        // Executando a validação
        var ehValido = Validator.TryValidateObject(usuarioInvalido, contexto, resultados, validateAllProperties: true);

        // A asserção espera que seja falso, forçando a falha para a demonstração
        Assert.false(ehValido);
    }
}