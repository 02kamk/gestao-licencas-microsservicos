using Licencas.API.Data;
using Licencas.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<IColaboradorClient, ColaboradorClient>(client =>
{
    var baseUrl = builder.Configuration["Services:ColaboradoresApi"];
    client.BaseAddress = new Uri(baseUrl ?? "https://localhost:7168");
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddScoped<ILicencaService, LicencaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
    GarantirColunaColaboradorId(dbContext);
}

app.Run();

static void GarantirColunaColaboradorId(AppDbContext dbContext)
{
    var connection = dbContext.Database.GetDbConnection();
    connection.Open();

    using var consulta = connection.CreateCommand();
    consulta.CommandText = "PRAGMA table_info('Licencas')";

    var colunaExiste = false;
    using (var reader = consulta.ExecuteReader())
    {
        while (reader.Read())
        {
            if (reader["name"]?.ToString() == "ColaboradorId")
            {
                colunaExiste = true;
                break;
            }
        }
    }

    if (colunaExiste)
    {
        return;
    }

    using var alteracao = connection.CreateCommand();
    alteracao.CommandText = "ALTER TABLE Licencas ADD COLUMN ColaboradorId INTEGER NOT NULL DEFAULT 0";
    alteracao.ExecuteNonQuery();
}
