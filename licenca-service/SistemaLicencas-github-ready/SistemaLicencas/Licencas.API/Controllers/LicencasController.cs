using Licencas.API.Models;
using Licencas.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Licencas.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LicencasController : ControllerBase
{
    private readonly ILicencaService _licencaService;

    public LicencasController(ILicencaService licencaService)
    {
        _licencaService = licencaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Licenca>>> GetLicencas()
    {
        var licencas = await _licencaService.ListarAsync();
        return Ok(licencas);
    }

    [HttpGet("colaborador/{colaboradorId}")]
    public async Task<ActionResult<IEnumerable<Licenca>>> GetLicencasPorColaborador(int colaboradorId)
    {
        var licencas = await _licencaService.ListarPorColaboradorAsync(colaboradorId);
        return Ok(licencas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Licenca>> GetLicenca(int id)
    {
        var licenca = await _licencaService.BuscarPorIdAsync(id);

        if (licenca == null)
        {
            return NotFound();
        }

        return licenca;
    }

    [HttpPost]
    public async Task<ActionResult<Licenca>> PostLicenca(Licenca licenca)
    {
        if (licenca.ValidaAte <= DateTime.UtcNow)
        {
            return BadRequest("A data de validade precisa ser futura.");
        }

        var resultado = await _licencaService.CriarAsync(licenca);

        if (!resultado.Sucesso)
        {
            return BadRequest(resultado.Erro);
        }

        return CreatedAtAction(nameof(GetLicenca), new { id = resultado.Dados!.Id }, resultado.Dados);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutLicenca(int id, Licenca licenca)
    {
        var resultado = await _licencaService.AtualizarAsync(id, licenca);

        if (!resultado.Sucesso && resultado.Erro == "Licenca nao encontrada.")
        {
            return NotFound();
        }

        if (!resultado.Sucesso)
        {
            return BadRequest(resultado.Erro);
        }

        return NoContent();
    }

    [HttpPost("{id}/ativar")]
    public async Task<ActionResult<Licenca>> AtivarLicenca(int id)
    {
        var licenca = await _licencaService.AtivarAsync(id);

        if (licenca == null)
        {
            return NotFound();
        }

        return licenca;
    }

    [HttpPost("{id}/cancelar")]
    public async Task<ActionResult<Licenca>> CancelarLicenca(int id)
    {
        var licenca = await _licencaService.CancelarAsync(id);

        if (licenca == null)
        {
            return NotFound();
        }

        return licenca;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLicenca(int id)
    {
        var removida = await _licencaService.RemoverAsync(id);

        if (!removida)
        {
            return NotFound();
        }

        return NoContent();
    }
}
