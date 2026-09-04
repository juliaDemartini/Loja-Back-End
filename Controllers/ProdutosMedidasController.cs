using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.Models;
using LojaBackEnd.DTOs;

namespace LojaBackEnd.Controllers;

[ApiController]
[Route("api/produtos/{id}/medidas")]
public class ProdutosMedidasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosMedidasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObterMedidas(int id)
    {
        var produto = await _context.Produtos
            .AsNoTracking()
            .Include(p => p.Medidas)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (produto == null)
        {
            return NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });
        }

        var ordemTamanhos = new List<string> { "PP", "P", "M", "G", "GG", "XG" };

        var medidasOrdenadas = produto.Medidas
            .OrderBy(m => {
                var idx = ordemTamanhos.IndexOf(m.Tamanho.ToUpper().Trim());
                return idx >= 0 ? idx : 99;
            })
            .Select(m => new MedidaItemDto
            {
                Tamanho = m.Tamanho,
                BustoMin = m.BustoMin,
                BustoMax = m.BustoMax,
                CinturaMin = m.CinturaMin,
                CinturaMax = m.CinturaMax,
                QuadrilMin = m.QuadrilMin,
                QuadrilMax = m.QuadrilMax,
                Comprimento = m.Comprimento
            })
            .ToList();

        var resposta = new TabelaMedidasResponseDto
        {
            ProdutoId = produto.Id,
            ProdutoNome = produto.Nome,
            Medidas = medidasOrdenadas
        };

        return Ok(resposta);
    }

    [HttpPost]
    public async Task<IActionResult> SalvarMedidasEmLote(int id, [FromBody] List<MedidaItemDto> medidasDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (medidasDto == null || medidasDto.Count == 0)
        {
            return BadRequest(new { mensagem = "A lista de medidas não pode estar vazia." });
        }

        var produto = await _context.Produtos
            .Include(p => p.Medidas)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (produto == null)
        {
            return NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });
        }

        // Substituição atômica: remove as medidas antigas do produto e cadastra a nova grade
        _context.MedidasProdutos.RemoveRange(produto.Medidas);

        var novasMedidas = medidasDto.Select(dto => new MedidaProduto
        {
            ProdutoId = id,
            Tamanho = dto.Tamanho.Trim().ToUpper(),
            BustoMin = dto.BustoMin,
            BustoMax = dto.BustoMax,
            CinturaMin = dto.CinturaMin,
            CinturaMax = dto.CinturaMax,
            QuadrilMin = dto.QuadrilMin,
            QuadrilMax = dto.QuadrilMax,
            Comprimento = dto.Comprimento
        }).ToList();

        await _context.MedidasProdutos.AddRangeAsync(novasMedidas);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = $"Grade de medidas do produto #{id} atualizada com sucesso.",
            totalTamanhosConfigurados = novasMedidas.Count
        });
    }
}