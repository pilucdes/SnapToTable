using Asp.Versioning;
using FluentValidation;
using Scalar.AspNetCore;
using SnapToTable.API.Behaviors;
using SnapToTable.API.Middlewares;
using SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;
using SnapToTable.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateRecipeAnalysisRequestCommand>();
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateRecipeAnalysisRequestCommand>();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1);
    opt.ReportApiVersions = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<GlobalErrorHandler>();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;