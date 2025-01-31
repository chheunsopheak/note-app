using Microsoft.EntityFrameworkCore;
using noted_api.Contexts;
using noted_api.Interface;
using noted_api.Service;

namespace noted_api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => { sqlServerOptions.EnableRetryOnFailure(); });
        });

        return services;
    }

    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<INoteService, NoteService>();
        return services;
    }
}