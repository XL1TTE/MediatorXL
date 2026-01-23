using MediatorXL.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddMediatorXL(cfg =>
{
    cfg.AssembliesWithHandlers = new[]
    {
        typeof(Program).Assembly,
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/scalar");
}

app.MapControllers();

app.UseHttpsRedirection();

app.UsePathBase("/slalar");
app.UseRouting();

app.Run();
