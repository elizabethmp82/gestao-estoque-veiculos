using EstoqueVeiculos.Api.Database;
using EstoqueVeiculos.Api.Repositories;
using EstoqueVeiculos.Api.Services;




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<OracleConnectionFactory>();
builder.Services.AddScoped<VeiculoRepository>();
builder.Services.AddScoped<VeiculoService>();
builder.Services.AddScoped<ProprietarioRepository>();
builder.Services.AddScoped<ProprietarioService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();



var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "API de Gestão de Estoque de Veículos");

app.MapGet("/teste-oracle", async (OracleConnectionFactory connectionFactory) =>
{
    using var connection = connectionFactory.CreateConnection();

    await connection.OpenAsync();

    using var command = connection.CreateCommand();

    command.CommandText = "SELECT 1 FROM DUAL";

    var resultado = await command.ExecuteScalarAsync();

    return Results.Ok(new
    {
        mensagem = "Conexão com Oracle realizada com sucesso.",
        resultado
    });
});





app.MapControllers();

app.Run();