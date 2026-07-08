using System.Net;
using System.Text.Json;
using GastosResidenciais.Api.Exceptions;

namespace GastosResidenciais.Api.Middleware;

/// <summary>
/// Middleware central de tratamento de erros. Evita repetir try/catch em cada
/// controller: cada tipo de exceção de negócio é mapeado para o status HTTP correto,
/// e a resposta é sempre um JSON padronizado { message: "..." }.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            var statusCode = ex switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                BusinessRuleException => HttpStatusCode.BadRequest,
                ArgumentException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(ex, "Erro não tratado ao processar a requisição.");
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var payload = JsonSerializer.Serialize(new
            {
                message = statusCode == HttpStatusCode.InternalServerError
                    ? "Ocorreu um erro interno no servidor."
                    : ex.Message
            });

            await context.Response.WriteAsync(payload);
        }
    }
}
