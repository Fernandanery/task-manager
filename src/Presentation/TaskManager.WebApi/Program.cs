using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using TaskManager.Application.Auth;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Application.Services.Auth;
using TaskManager.Application.Validators;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;
using TaskManager.WebApi.SwaggerExamples;

var builder = WebApplication.CreateBuilder(args);

// Config hangfire
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
          {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
              QueuePollInterval = TimeSpan.Zero,
              UseRecommendedIsolationLevel = true,
              DisableGlobalLocks = true
          }));

builder.Services.AddHangfireServer();

// Configurando o Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Config mapeamento (Mapster)
MappingConfig.RegisterMappings();

// Reutilizando instâncias de DbContext
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependência
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

// JWT Token Service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TaskValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Mapster
builder.Services.AddMapster();
builder.Services.AddSingleton<IMapper, Mapper>();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Autenticação JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
    });

// Swagger + JWT + Examples
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Manager API",
        Version = "v1",
        Description = "API para gerenciamento de tarefas"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<BadRequestProblemDetailsExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<NotFoundProblemDetailsExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<InternalServerProblemDetailsExample>();

// Hash de senha
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

// Hangfire
app.UseHangfireDashboard();

// Middleware de tratamento de exceções
app.UseMiddleware<ExceptionMiddleware>();

// Middleware TraceId
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

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapScalarApiReference(options =>
        {
            options.Title = "Task Manager API";
            options.OpenApiRoutePattern = "/swagger/v1/swagger.json";
        });

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

// Hangfire jobs
RecurringJob.AddOrUpdate<TaskReminderJob>(
    "lembrete-tarefas-pendentes",
    job => job.EnviarLembretesDeTarefasPendentes(),
    "0 8 * * *");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();