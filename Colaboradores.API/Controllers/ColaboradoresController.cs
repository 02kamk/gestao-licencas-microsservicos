using Colaboradores.API.Data;
using Colaboradores.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Colaboradores.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColaboradoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public ColaboradoresController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/colaboradores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Colaborador>>> GetColaboradores()
    {
        return await _context.Colaboradores.ToListAsync();
    }

    // GET: api/colaboradores/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Colaborador>> GetColaborador(int id)
    {
        var colaborador = await _context.Colaboradores.FindAsync(id);
        if (colaborador == null)
            return NotFound();
        return colaborador;
    }

    // POST: api/colaboradores
    [HttpPost]
    public async Task<ActionResult<Colaborador>> PostColaborador(Colaborador colaborador)
    {
        _context.Colaboradores.Add(colaborador);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetColaborador), new { id = colaborador.Id }, colaborador);
    }

    // PUT: api/colaboradores/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutColaborador(int id, Colaborador colaborador)
    {
        if (id != colaborador.Id)
            return BadRequest();

        _context.Entry(colaborador).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/colaboradores/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteColaborador(int id)
    {
        var colaborador = await _context.Colaboradores.FindAsync(id);
        if (colaborador == null)
            return NotFound();

        _context.Colaboradores.Remove(colaborador);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}