using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.Models;
using LojaBackEnd.DTOs;

namespace LojaBackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly AppDbContext _context;

    public PedidosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Inicia a transação atômica
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var emailNormalizado = dto.Cliente.Email.Trim().ToLower();

            // 1. Localiza ou cria o usuário na base
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == emailNormalizado);

            if (usuario == null)
            {
                // Senha temporária com hash seguro para checkout direto
                var senhaTemporariaHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString("N"));

                usuario = new Usuario
                {
                    Nome = dto.Cliente.Nome.Trim(),
                    Email = emailNormalizado,
                    SenhaHash = senhaTemporariaHash,
                    Telefone = dto.Cliente.Telefone?.Trim(),
                    Cpf = dto.Cliente.Cpf?.Trim(),
                    DataCriacao = DateTime.UtcNow
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Atualiza telefone/CPF caso venham preenchidos
                if (string.IsNullOrWhiteSpace(usuario.Telefone) && !string.IsNullOrWhiteSpace(dto.Cliente.Telefone))
                    usuario.Telefone = dto.Cliente.Telefone.Trim();

                if (string.IsNullOrWhiteSpace(usuario.Cpf) && !string.IsNullOrWhiteSpace(dto.Cliente.Cpf))
                    usuario.Cpf = dto.Cliente.Cpf.Trim();

                await _context.SaveChangesAsync();
            }

            // 2. Instancia a entidade Pedido
            var statusInicial = dto.FormaPagamento.ToUpper() == "PIX" ? "aguardando_pagamento" : "pago";

            var pedido = new Pedido
            {
                UsuarioId = usuario.Id,
                DataCriacao = DateTime.UtcNow,
                Subtotal = dto.Subtotal,
                Frete = dto.ValorFrete,
                Desconto = dto.DescontoPix,
                Total = dto.TotalFinal,
                Cep = dto.Endereco.Cep.Trim(),
                Logradouro = dto.Endereco.Logradouro.Trim(),
                Numero = dto.Endereco.Numero.Trim(),
                Complemento = dto.Endereco.Complemento?.Trim(),
                Bairro = dto.Endereco.Bairro.Trim(),
                Cidade = dto.Endereco.Cidade.Trim(),
                Uf = dto.Endereco.Uf.Trim().ToUpper(),
                TipoFrete = dto.TipoFrete,
                FormaPagamento = dto.FormaPagamento.ToUpper(),
                Status = statusInicial
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync(); // Gera o Id do pedido

            // 3. Cadastra os Itens vinculados ao Pedido
            foreach (var item in dto.Itens)
            {
                var itemPedido = new ItemPedido
                {
                    PedidoId = pedido.Id,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    Tamanho = string.IsNullOrWhiteSpace(item.Tamanho) ? "M" : item.Tamanho
                };

                _context.ItensPedido.Add(itemPedido);
            }

            await _context.SaveChangesAsync();

            // 4. Confirma todas as operações no banco de uma só vez
            await transaction.CommitAsync();

            // 5. Retorna o contrato consumido pelo Frontend e pela tela /minha-conta
            var resposta = new PedidoCriadoRespostaDto
            {
                Id = pedido.Id.ToString(),
                Data = pedido.DataCriacao,
                Status = pedido.Status,
                TotalFinal = pedido.Total,
                Subtotal = pedido.Subtotal,
                ValorFrete = pedido.Frete,
                DescontoPix = pedido.Desconto,
                FormaPagamento = pedido.FormaPagamento,
                TipoFrete = pedido.TipoFrete,
                Cliente = new ClienteCheckoutDto
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Telefone = usuario.Telefone,
                    Cpf = usuario.Cpf
                },
                Endereco = dto.Endereco,
                Itens = dto.Itens
            };

            return StatusCode(201, resposta);
        }
        catch (Exception ex)
        {
            // Em caso de erro, desfaz qualquer alteração
            await transaction.RollbackAsync();
            return StatusCode(500, new { mensagem = "Erro ao processar o checkout.", detalhe = ex.Message });
        }
    }
    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> ObterPorUsuario(int usuarioId)
    {
        // 1. Busca os pedidos incluindo Itens e Produtos, ordenados pelos mais recentes
        var pedidos = await _context.Pedidos
            .AsNoTracking()
            .Where(p => p.UsuarioId == usuarioId)
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync();

        // 2. Mapeamento para o DTO de histórico com as fotos dos produtos
        var resposta = pedidos.Select(p => new PedidoHistoricoDto
        {
            Id = p.Id,
            DataCriacao = p.DataCriacao,
            Status = p.Status,
            Total = p.Total,
            Subtotal = p.Subtotal,
            Frete = p.Frete,
            Desconto = p.Desconto,
            FormaPagamento = p.FormaPagamento,
            TipoFrete = p.TipoFrete,
            Itens = p.Itens.Select(i => new ItemPedidoHistoricoDto
            {
                ProdutoId = i.ProdutoId,
                Nome = i.Produto != null ? i.Produto.Nome : "Produto indisponível",
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                Tamanho = i.Tamanho,
                ImagemUrl = i.Produto?.ImagemUrl
            }).ToList()
        }).ToList();

        return Ok(resposta);
    
    }
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusPedidoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
        {
            return NotFound(new { mensagem = $"Pedido com ID {id} não foi encontrado." });
        }

        // Garante que ambos sejam tratados estritamente como string
        string statusAtual = (pedido.Status ?? string.Empty).Trim().ToLower();
        string novoStatus = (dto.Status ?? string.Empty).Trim().ToLower();

        // Normalização de nomes equivalentes
        if (statusAtual == "aguardando_pagamento") statusAtual = "pendente";
        if (novoStatus == "aguardando_pagamento") novoStatus = "pendente";

        // Tabela de transições válidas
        var transicoesValidas = new Dictionary<string, List<string>>
        {
            { "pendente", new List<string> { "pago", "cancelado" } },
            { "pago", new List<string> { "enviado", "cancelado" } },
            { "enviado", new List<string> { "entregue" } },
            { "entregue", new List<string>() },
            { "cancelado", new List<string>() }
        };

        if (!transicoesValidas.ContainsKey(statusAtual))
        {
            return BadRequest(new { mensagem = $"Status atual '{pedido.Status}' é inválido." });
        }

        if (!transicoesValidas.ContainsKey(novoStatus))
        {
            return BadRequest(new { mensagem = $"O novo status '{dto.Status}' não é reconhecido pelo sistema." });
        }

        // Validação da transição
        if (statusAtual != novoStatus && !transicoesValidas[statusAtual].Contains(novoStatus))
        {
            return BadRequest(new
            {
                mensagem = $"Transição inválida: não é permitido alterar o status de '{pedido.Status}' diretamente para '{dto.Status}'."
            });
        }
// Atualização dos dados e timestamp com proteção contra nulos
        pedido.Status = char.ToUpper(novoStatus[0]) + (novoStatus.Length > 1 ? novoStatus[1..] : "");
        pedido.CodigoRastreio = dto.CodigoRastreio?.Trim().ToUpper();
        pedido.DataAtualizacao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Status do pedido atualizado com sucesso.",
            pedidoId = pedido.Id,
            status = pedido.Status,
            codigoRastreio = pedido.CodigoRastreio,
            dataAtualizacao = pedido.DataAtualizacao
        });
    }
    
}