using EstoqueVeiculos.Api.Exceptions;
using Oracle.ManagedDataAccess.Client;
using System.Net;
using System.Text.Json;

namespace EstoqueVeiculos.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await EscreverRespostaAsync(
                context,
                HttpStatusCode.NotFound,
                ex.Message
            );
        }
        catch (ArgumentException ex)
        {
            await EscreverRespostaAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message
            );
        }
        catch (OracleException ex)
        {
            _logger.LogError(
                ex,
                "Erro ao acessar o banco de dados Oracle."
            );

            await EscreverRespostaAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro ao acessar o banco de dados."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro inesperado na aplicação."
            );

            await EscreverRespostaAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno no servidor."
            );
        }
    }

    private static async Task EscreverRespostaAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string mensagem)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var resposta = new
        {
            mensagem
        };

        var json = JsonSerializer.Serialize(resposta);

        await context.Response.WriteAsync(json);
    }
}