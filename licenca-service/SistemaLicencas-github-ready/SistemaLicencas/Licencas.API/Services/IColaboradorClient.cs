using Licencas.API.Models;

namespace Licencas.API.Services;

public interface IColaboradorClient
{
    Task<ColaboradorResumo?> BuscarPorIdAsync(int id);
}
