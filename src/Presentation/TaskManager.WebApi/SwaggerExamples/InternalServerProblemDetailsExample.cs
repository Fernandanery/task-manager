using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.WebApi.SwaggerExamples;

public class InternalServerProblemDetailsExample
{
    public ProblemDetails GetExamples()
    {
        return new ValidationProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
            Detail = Miscellaneous.StatusCode500,
            Type = "https://meuapp.com/docs/errors/internal-server-error"
        };
    }
}
