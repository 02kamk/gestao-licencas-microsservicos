namespace Licencas.API.Services;

public class ResultadoOperacao<T>
{
    public bool Sucesso { get; private init; }
    public string? Erro { get; private init; }
    public T? Dados { get; private init; }

    public static ResultadoOperacao<T> Ok(T dados)
    {
        return new ResultadoOperacao<T> { Sucesso = true, Dados = dados };
    }

    public static ResultadoOperacao<T> Falha(string erro)
    {
        return new ResultadoOperacao<T> { Sucesso = false, Erro = erro };
    }
}
