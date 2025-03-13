using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Swashbuckle.AspNetCore.Filters;

namespace TaskManager.WebApi.SwaggerExamples;

public class BadRequestProblemDetailsExample : IExamplesProvider<ValidationProblemDetails>
{
    public ValidationProblemDetails GetExamples()
    {
        return new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            { "Name", new[] { "O campo nome é obrigatório." } }
        })
        {
            Status = StatusCodes.Status400BadRequest,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
            Type = "https://meuapp.com/docs/errors/bad-request"
        };
    }
}
