using BlazorBlog.Features.Admin.Services;

namespace BlazorBlog.Features.Admin.Configurations;

/// <summary>
/// Configuration des services pour la fonctionnalité d'administration.
/// </summary>
public static class AdminServiceConfiguration
{
    /// <summary>
    /// Ajoute les services nécessaires pour la page d'administration.
    /// </summary>
    /// <param name="services">La collection de services.</param>
    /// <returns>La collection de services pour le chaînage.</returns>
    public static IServiceCollection AddAdminServices(this IServiceCollection services)
    {
        // Page ViewService
        services.AddScoped<IAdminPageViewService, AdminPageViewService>();

        return services;
    }
}

