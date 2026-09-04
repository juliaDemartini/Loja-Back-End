using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.DTOs;

namespace LojaBackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("verificar")]
    public async Task<IActionResult> VerificarEmail([FromQuery] string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new { mensagem = "O parâmetro 'email' é obrigatório." });
        }

        var emailValidador = new EmailAddressAttribute();
        if (!emailValidador.IsValid(email))
        {
            return BadRequest(new { mensagem = "O formato do e-mail informado é inválido." });
        }

        var emailNormalizado = email.Trim().ToLower();

        // Busca otimizada sem tracking de memória, trazendo apenas a coluna necessária
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .Where(u => u.Email.ToLower() == emailNormalizado)
            .Select(u => new { u.Nome })
            .FirstOrDefaultAsync();

        if (usuario == null)
        {
            return Ok(new ClienteVerificacaoDto
            {
                Existe = false,
                Nome = null
            });
        }

        // Pega apenas o primeiro nome para personalização segura no checkout
        var primeiroNome = usuario.Nome.Trim().Split(' ')[0];

        return Ok(new ClienteVerificacaoDto
        {
            Existe = true,
            Nome = primeiroNome
        });
    }
}