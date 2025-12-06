using BlazorBlog.Features.Home.Services;

namespace BlazorBlog.Features.Home.Configurations;

/// <summary>
/// Configuration des services pour la fonctionnalité de la page d'accueil.
/// </summary>
public static class HomeServiceConfiguration
{
    /// <summary>
    /// Ajoute les services nécessaires pour la page d'accueil.
    /// </summary>
    /// <param name="services">La collection de services.</param>
    /// <returns>La collection de services pour le chaînage.</returns>
    public static IServiceCollection AddHomeServices(this IServiceCollection services)
    {
        // Page ViewService
        services.AddScoped<IHomePageViewService, HomePageViewService>();

        return services;
    }
}

