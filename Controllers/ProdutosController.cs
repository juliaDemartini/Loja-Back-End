using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaBackEnd.Data;
using LojaBackEnd.Models;

namespace LojaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Ou se preferir usar apenas "[Route("produtos")]", ajuste conforme sua rota atual
    [Route("produtos")] 
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos
                .Include(p => p.Medidas)
                .ToListAsync();
        }

        // GET: /produtos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProdutoPorId(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.Medidas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // POST: /produtos
        [HttpPost]
        public async Task<ActionResult<Produto>> CriarProduto([FromBody] Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProdutoPorId), new { id = produto.Id }, produto);
        }

        // PUT: /produtos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] Produto produtoDto)
        {
            var produtoExistente = await _context.Produtos.FindAsync(id);
            if (produtoExistente == null)
            {
                return NotFound();
            }

            produtoExistente.Nome = produtoDto.Nome;
            produtoExistente.Preco = produtoDto.Preco;
            produtoExistente.ImagemUrl = produtoDto.ImagemUrl;
            
            // Novos campos de atributos e provador 3D
            produtoExistente.Categoria = produtoDto.Categoria;
            produtoExistente.Tipo = produtoDto.Tipo;
            produtoExistente.Estilo = produtoDto.Estilo;
            produtoExistente.Lavagem = produtoDto.Lavagem;
            produtoExistente.Provador3dDisponivel = produtoDto.Provador3dDisponivel;

            await _context.SaveChangesAsync();

            return Ok(produtoExistente);
        }

        // DELETE: /produtos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}