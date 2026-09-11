using EstoqueVeiculos.Api.Database;
using EstoqueVeiculos.Api.Repositories;
using EstoqueVeiculos.Api.Services;
using EstoqueVeiculos.Api.Middleware;


var builder = WebApplication.CreateBuilder(args);
// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conexão com Oracle
builder.Services.AddSingleton<OracleConnectionFactory>();

// Repositories
builder.Services.AddScoped<VeiculoRepository>();
builder.Services.AddScoped<ProprietarioRepository>();

// Services
builder.Services.AddScoped<VeiculoService>();
builder.Services.AddScoped<ProprietarioService>();


var app = builder.Build();

// Swagger somente em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware global para tratamento de exceções
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


