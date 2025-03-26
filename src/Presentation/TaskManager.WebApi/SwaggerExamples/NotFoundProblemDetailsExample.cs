using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Swashbuckle.AspNetCore.Filters;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.WebApi.SwaggerExamples;

public class NotFoundProblemDetailsExample
{
    public ProblemDetails GetExamples()
    {
        return new ValidationProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status404NotFound),
            Detail = Miscellaneous.StatusCode404,
            Type = "https://meuapp.com/docs/errors/not-found"
        };
    }
}
