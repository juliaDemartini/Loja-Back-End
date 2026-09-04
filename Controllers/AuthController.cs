using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.Models;
using LojaBackEnd.DTOs;

namespace LojaBackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("cadastro")]
    public async Task<IActionResult> Cadastrar([FromBody] CadastroUsuarioDto dto)
    {
        var emailNormalizado = dto.Email.Trim().ToLower();

        var emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email.ToLower() == emailNormalizado);

        if (emailExiste)
        {
            return BadRequest(new { mensagem = "Já existe uma conta com este e-mail." });
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

        var novoUsuario = new Usuario
        {
            Nome = dto.Nome.Trim(),
            Email = emailNormalizado,
            SenhaHash = hash,
            Telefone = dto.Telefone?.Trim(),
            Cpf = dto.Cpf?.Trim(),
            DataCriacao = DateTime.UtcNow
        };

        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();

        var resposta = new UsuarioRespostaDto
        {
            Id = novoUsuario.Id,
            Nome = novoUsuario.Nome,
            Email = novoUsuario.Email,
            Telefone = novoUsuario.Telefone,
            Cpf = novoUsuario.Cpf
        };

        return StatusCode(201, resposta);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioDto dto)
    {
        var emailNormalizado = dto.Email.Trim().ToLower();

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.ToLower() == emailNormalizado);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
        {
            return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
        }

        var resposta = new UsuarioRespostaDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Telefone = usuario.Telefone,
            Cpf = usuario.Cpf
        };

        return Ok(resposta);
    }
}