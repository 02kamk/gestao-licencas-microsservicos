namespace Licencas.API.Models;

public class Licenca
{
    public int Id { get; set; }
    public int ColaboradorId { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Produto { get; set; } = string.Empty;
    public string Chave { get; set; } = string.Empty;
    public StatusLicenca Status { get; set; } = StatusLicenca.Pendente;
    public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
    public DateTime ValidaAte { get; set; }
    public DateTime? AtivadaEm { get; set; }
    public DateTime? CanceladaEm { get; set; }
}
