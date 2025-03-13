using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace TaskManager.WebApi.Configuration;

public static class ProblemDetailsConfig
{
    public static void ConfigureProblemDetails(IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (ctx, ex) =>
            {
                var env = ctx.RequestServices.GetRequiredService<IWebHostEnvironment>();
                return env.IsDevelopment();
            };

            options.Map<BadHttpRequestException>((ctx, ex) =>
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
                    Detail = ex.Message,
                    Type = "https://meuapp.com/docs/errors/bad-request",
                    Extensions =
                    {
                        ["traceId"] = ctx.TraceIdentifier
                    }
                });

            options.Map<ValidationException>((ctx, ex) =>
                new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "ValidationErrors", ex.Errors.Select(x => x.ErrorMessage).ToArray() }
                })
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Erro de validação",
                    Detail = "Os dados enviados contêm erros de validação.",
                    Type = "https://meuapp.com/docs/errors/validation-error",
                    Extensions =
                    {
                        ["traceId"] = ctx.TraceIdentifier
                    }
                });

            options.Map<KeyNotFoundException>((ctx, ex) =>
                new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = ex.Message ?? "O recurso solicitado não foi encontrado.",
                    Type = "https://meuapp.com/docs/errors/not-found",
                    Extensions =
                    {
            ["traceId"] = ctx.TraceIdentifier
                    }
                });


            options.Map<Exception>((ctx, ex) =>
                new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
                    Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                    Type = "https://meuapp.com/docs/errors/internal-server-error",
                    Extensions =
                    {
                        ["traceId"] = ctx.TraceIdentifier
                    }
                });
        });
    }
}
