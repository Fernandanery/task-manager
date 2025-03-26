using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Swashbuckle.AspNetCore.Filters;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.WebApi.SwaggerExamples;

public class BadRequestProblemDetailsExample
{
    public ProblemDetails GetExamples()
    {
        return new ValidationProblemDetails()  
        {
            Status = StatusCodes.Status400BadRequest,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
            Type = "https://meuapp.com/docs/errors/bad-request",
            Detail = Miscellaneous.StatusCode400
        };
    }
}
