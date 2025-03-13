using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Application.Validators;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;
using TaskManager.WebApi.Configuration;
using TaskManager.WebApi.SwaggerExamples;

var builder = WebApplication.CreateBuilder(args);

// Configura ProblemDetails
ProblemDetailsConfig.ConfigureProblemDetails(builder.Services);

// Config mapeamento (Mapster)
MappingConfig.RegisterMappings();

// Config Entity Framework e SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Config injeção de dependência para repositórios e serviços
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

// Config FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TaskValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Config Mapster
builder.Services.AddMapster();
builder.Services.AddSingleton<IMapper, Mapper>();

// Config Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configuração do Swagger com exemplos
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Manager API",
        Version = "v1",
        Description = "API para gerenciamento de tarefas"
    });

    //  Adiciona os exemplos personalizados
    c.ExampleFilters();
});

// Carrega exemplos de erro no Swagger
builder.Services.AddSwaggerExamplesFromAssemblyOf<BadRequestProblemDetailsExample>();

var app = builder.Build();

// Usa ProblemDetails Middleware
app.UseProblemDetails();

// Adiciona TraceId automaticamente no Response
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.Append("X-Trace-Id", context.TraceIdentifier);
        return Task.CompletedTask;
    });

    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Redireciona para o Swagger automaticamente
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }

        await next();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
