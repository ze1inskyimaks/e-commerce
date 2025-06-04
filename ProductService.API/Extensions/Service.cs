using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure;

namespace ProductService.API.Extensions;

public static class Service
{
    public static IServiceCollection AddDataBase(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        return services;
    }

    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    { 
        return services;
    }
    
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
    
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        return builder;
    }
    
    public static WebApplication AddApplicationSettings(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.MapControllers();

        return app;
    }
}