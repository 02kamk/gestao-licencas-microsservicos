using System.Net;
using Licencas.API.Models;

namespace Licencas.API.Services;

public class ColaboradorClient : IColaboradorClient
{
    private readonly HttpClient _httpClient;

    public ColaboradorClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ColaboradorResumo?> BuscarPorIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/colaboradores/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ColaboradorResumo>();
    }
}
