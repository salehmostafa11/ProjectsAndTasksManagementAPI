using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Projects_And_Tasks_Management_WebAPI.Data;
using Projects_And_Tasks_Management_WebAPI.Exceptions;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectRequests;

namespace Projects_And_Tasks_Management_WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddControllersToApp();
        services.AddExceptionHandiling();
        services.AddFluentValidation();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source = app.db";

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        return services;
    }

    public static IServiceCollection AddControllersToApp(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        
        return services;
    }

    public static IServiceCollection AddExceptionHandiling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalexceptionHandeler>();
        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateProjectRequest>();
        return services;
    }
}