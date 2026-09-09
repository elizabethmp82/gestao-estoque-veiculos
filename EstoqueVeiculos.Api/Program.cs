using EstoqueVeiculos.Api.Database;
using EstoqueVeiculos.Api.Repositories;
using EstoqueVeiculos.Api.Services;
using EstoqueVeiculos.Api.DTOs;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<OracleConnectionFactory>();
builder.Services.AddScoped<VeiculoRepository>();
builder.Services.AddScoped<VeiculoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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

app.MapGet("/veiculos", async (
    string? marca,
    string? situacao,
    VeiculoRepository repository) =>
{
    var veiculos = await repository.ListarAsync(marca, situacao);

    return Results.Ok(veiculos);
});

app.MapPost("/veiculos", async (
    VeiculoCreateDto dto,
    VeiculoService service) =>
{
    try
    {
        await service.CriarAsync(dto);

        return Results.Created(
            "/veiculos",
            new { mensagem = "Veículo cadastrado com sucesso." }
        );
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new
        {
            mensagem = ex.Message
        });
    }
});

app.MapGet("/veiculos/{id:int}", async (
    int id,
    VeiculoRepository repository) =>
{
    var veiculo = await repository.BuscarPorIdAsync(id);

    if (veiculo is null)
    {
        return Results.NotFound(new
        {
            mensagem = "Veículo não encontrado."
        });
    }

    return Results.Ok(veiculo);
});

app.Run();