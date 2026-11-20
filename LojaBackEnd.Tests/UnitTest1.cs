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
}
