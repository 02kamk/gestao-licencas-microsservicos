# SistemaLicencas

Solucao limpa contendo apenas o microservico `Licencas.API`.

## Como rodar

1. Abra `SistemaLicencas.slnx` no Visual Studio.
2. Clique com o botao direito em `Licencas.API`.
3. Escolha `Definir como Projeto de Inicializacao`.
4. No botao verde, selecione o perfil `https`.
5. Execute.

Swagger:

- `https://localhost:7079/swagger`

## Integracao com Colaboradores.API

Este projeto nao inclui `Colaboradores.API` na mesma solucao para evitar que o Visual Studio rode o projeto errado.

Mesmo assim, a integracao continua ativa. Antes de criar ou atualizar uma licenca, `Licencas.API` consulta:

- `https://localhost:7168/api/colaboradores/{id}`

Essa URL fica configurada em `Licencas.API/appsettings.json`:

```json
{
  "Services": {
    "ColaboradoresApi": "https://localhost:7168"
  }
}
```

Para cadastrar uma licenca, deixe a `Colaboradores.API` rodando e envie:

```json
{
  "colaboradorId": 1,
  "cliente": "Empresa Exemplo",
  "produto": "Sistema ERP",
  "validaAte": "2026-12-31T23:59:59"
}
```

O banco SQLite `licencas.db` e criado automaticamente na primeira execucao.
