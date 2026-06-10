using Licencas.API.Data;
using Licencas.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Licencas.API.Services;

public class LicencaService : ILicencaService
{
    private readonly AppDbContext _context;
    private readonly IColaboradorClient _colaboradorClient;

    public LicencaService(AppDbContext context, IColaboradorClient colaboradorClient)
    {
        _context = context;
        _colaboradorClient = colaboradorClient;
    }

    public async Task<IEnumerable<Licenca>> ListarAsync()
    {
        return await _context.Licencas
            .OrderByDescending(l => l.CriadaEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<Licenca>> ListarPorColaboradorAsync(int colaboradorId)
    {
        return await _context.Licencas
            .Where(l => l.ColaboradorId == colaboradorId)
            .OrderByDescending(l => l.CriadaEm)
            .ToListAsync();
    }

    public async Task<Licenca?> BuscarPorIdAsync(int id)
    {
        return await _context.Licencas.FindAsync(id);
    }

    public async Task<ResultadoOperacao<Licenca>> CriarAsync(Licenca licenca)
    {
        var colaboradorValido = await ValidarColaboradorAsync(licenca.ColaboradorId);

        if (!colaboradorValido.Sucesso)
        {
            return ResultadoOperacao<Licenca>.Falha(colaboradorValido.Erro!);
        }

        licenca.Id = 0;
        licenca.Chave = string.IsNullOrWhiteSpace(licenca.Chave)
            ? GerarChave()
            : licenca.Chave.Trim().ToUpperInvariant();
        licenca.Status = StatusLicenca.Pendente;
        licenca.CriadaEm = DateTime.UtcNow;

        _context.Licencas.Add(licenca);
        await _context.SaveChangesAsync();
        return ResultadoOperacao<Licenca>.Ok(licenca);
    }

    public async Task<ResultadoOperacao<bool>> AtualizarAsync(int id, Licenca licenca)
    {
        var licencaExistente = await _context.Licencas.FindAsync(id);

        if (licencaExistente == null)
        {
            return ResultadoOperacao<bool>.Falha("Licenca nao encontrada.");
        }

        var colaboradorValido = await ValidarColaboradorAsync(licenca.ColaboradorId);

        if (!colaboradorValido.Sucesso)
        {
            return ResultadoOperacao<bool>.Falha(colaboradorValido.Erro!);
        }

        licencaExistente.ColaboradorId = licenca.ColaboradorId;
        licencaExistente.Cliente = licenca.Cliente;
        licencaExistente.Produto = licenca.Produto;
        licencaExistente.ValidaAte = licenca.ValidaAte;

        await _context.SaveChangesAsync();
        return ResultadoOperacao<bool>.Ok(true);
    }

    public async Task<Licenca?> AtivarAsync(int id)
    {
        var licenca = await _context.Licencas.FindAsync(id);

        if (licenca == null)
        {
            return null;
        }

        if (licenca.ValidaAte < DateTime.UtcNow)
        {
            licenca.Status = StatusLicenca.Expirada;
        }
        else
        {
            licenca.Status = StatusLicenca.Ativa;
            licenca.AtivadaEm = DateTime.UtcNow;
            licenca.CanceladaEm = null;
        }

        await _context.SaveChangesAsync();
        return licenca;
    }

    public async Task<Licenca?> CancelarAsync(int id)
    {
        var licenca = await _context.Licencas.FindAsync(id);

        if (licenca == null)
        {
            return null;
        }

        licenca.Status = StatusLicenca.Cancelada;
        licenca.CanceladaEm = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return licenca;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var licenca = await _context.Licencas.FindAsync(id);

        if (licenca == null)
        {
            return false;
        }

        _context.Licencas.Remove(licenca);
        await _context.SaveChangesAsync();
        return true;
    }

    private static string GerarChave()
    {
        return $"LIC-{Guid.NewGuid():N}".ToUpperInvariant();
    }

    private async Task<ResultadoOperacao<bool>> ValidarColaboradorAsync(int colaboradorId)
    {
        if (colaboradorId <= 0)
        {
            return ResultadoOperacao<bool>.Falha("Informe um colaboradorId valido.");
        }

        ColaboradorResumo? colaborador;

        try
        {
            colaborador = await _colaboradorClient.BuscarPorIdAsync(colaboradorId);
        }
        catch (HttpRequestException)
        {
            return ResultadoOperacao<bool>.Falha("Nao foi possivel conectar na Colaboradores.API. Verifique se ela esta rodando em https://localhost:7168.");
        }
        catch (TaskCanceledException)
        {
            return ResultadoOperacao<bool>.Falha("A Colaboradores.API demorou para responder. Verifique se ela esta rodando em https://localhost:7168.");
        }

        if (colaborador == null)
        {
            return ResultadoOperacao<bool>.Falha("Colaborador nao encontrado na Colaboradores.API.");
        }

        if (!colaborador.Status)
        {
            return ResultadoOperacao<bool>.Falha("Nao e possivel criar licenca para colaborador inativo.");
        }

        return ResultadoOperacao<bool>.Ok(true);
    }
}
