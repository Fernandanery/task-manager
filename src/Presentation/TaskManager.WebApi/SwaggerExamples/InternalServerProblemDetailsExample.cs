using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Swashbuckle.AspNetCore.Filters;

namespace TaskManager.WebApi.SwaggerExamples;

public class InternalServerProblemDetailsExample : IExamplesProvider<ProblemDetails>
{
    public ProblemDetails GetExamples()
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
            Detail = "Erro inesperado no servidor.",
            Type = "https://meuapp.com/docs/errors/internal-server-error"
        };
    }
}
