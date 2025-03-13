using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Swashbuckle.AspNetCore.Filters;

namespace TaskManager.WebApi.SwaggerExamples;

public class NotFoundProblemDetailsExample : IExamplesProvider<ProblemDetails>
{
    public ProblemDetails GetExamples()
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status404NotFound),
            Detail = "O recurso solicitado não foi encontrado.",
            Type = "https://meuapp.com/docs/errors/not-found"
        };
    }
}
