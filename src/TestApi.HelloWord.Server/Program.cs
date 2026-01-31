using System.Diagnostics.CodeAnalysis;
using Scalar.AspNetCore;

namespace TestApi.HelloWord.Server;

[ExcludeFromCodeCoverage]
public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        // Add health checks
        builder.Services.AddHealthChecks();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        // Pouze pro production použijte HTTPS redirect
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();
        app.MapControllers();

        // Health check endpoints
        app.MapHealthChecks("/health");

        await app.RunAsync();
    }
}