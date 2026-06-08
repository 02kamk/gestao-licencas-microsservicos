namespace Colaboradores.API.Models;

public class Colaborador
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Setor { get; set; } = string.Empty;
    public bool Status { get; set; } = true; // Ativo padrão
}