using Licencas.API.Models;

namespace Licencas.API.Services;

public interface ILicencaService
{
    Task<IEnumerable<Licenca>> ListarAsync();
    Task<IEnumerable<Licenca>> ListarPorColaboradorAsync(int colaboradorId);
    Task<Licenca?> BuscarPorIdAsync(int id);
    Task<ResultadoOperacao<Licenca>> CriarAsync(Licenca licenca);
    Task<ResultadoOperacao<bool>> AtualizarAsync(int id, Licenca licenca);
    Task<Licenca?> AtivarAsync(int id);
    Task<Licenca?> CancelarAsync(int id);
    Task<bool> RemoverAsync(int id);
}
