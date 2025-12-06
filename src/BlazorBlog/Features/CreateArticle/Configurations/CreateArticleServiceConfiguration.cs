using BlazorBlog.Core.Configurations;
using BlazorBlog.Features.CreateArticle.Repository;
using BlazorBlog.Features.CreateArticle.Services;
using MongoDB.Driver;

namespace BlazorBlog.Features.CreateArticle.Configurations;

/// <summary>
/// Configuration des services pour la fonctionnalité de création d'articles.
/// </summary>
public static class CreateArticleServiceConfiguration
{
    /// <summary>
    /// Ajoute les services nécessaires pour la création d'articles (MongoDB, Repository, ViewService).
    /// </summary>
    /// <param name="services">La collection de services.</param>
    /// <returns>La collection de services pour le chaînage.</returns>
    public static IServiceCollection AddCreateArticleServices(this IServiceCollection services)
    {
        // Configuration MongoDB - récupère les settings depuis le service provider
        services.AddSingleton<IMongoClient>(sp =>
        {
            IApplicationSettings settings = sp.GetRequiredService<IApplicationSettings>();
            MongoClientSettings mongoSettings = MongoClientSettings.FromConnectionString(settings.MongoConnectionString);
            return new MongoClient(mongoSettings);
        });

        // Repository
        services.AddScoped<IArticleRepository, ArticleRepository>();

        // ViewService
        services.AddScoped<IArticleViewService, ArticleViewService>();
        
        // Page ViewService
        services.AddScoped<ICreateArticlePageViewService, CreateArticlePageViewService>();

        return services;
    }
}

