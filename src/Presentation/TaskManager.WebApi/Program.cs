using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using TaskManager.Application.Auth;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Application.Services.Auth;
using TaskManager.Application.Validators;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;
using TaskManager.WebApi.SwaggerExamples;

var builder = WebApplication.CreateBuilder(args);


// Config mapeamento (Mapster)
MappingConfig.RegisterMappings();

// Reutilizando instancias de DbContext
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
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

    // Adiciona suporte ao JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite **Bearer** e em seguida o token JWT.\n\nExemplo: `Bearer eyJhbGciOi...`"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    //Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    //  Adiciona os exemplos personalizados
    c.ExampleFilters();
});

// Carrega exemplos de erro no Swagger
builder.Services.AddSwaggerExamplesFromAssemblyOf<BadRequestProblemDetailsExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<NotFoundProblemDetailsExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<InternalServerProblemDetailsExample>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

//Config JWT
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


var app = builder.Build();


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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
