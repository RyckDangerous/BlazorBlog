using BlazorBlog.Core.Models;

namespace BlazorBlog.Features.Home.Services;

/// <summary>
/// Interface pour le service de vue de la page d'accueil.
/// </summary>
public interface IHomePageViewService
{
    /// <summary>
    /// La liste des articles à afficher.
    /// </summary>
    List<Article> Articles { get; }

    /// <summary>
    /// Indique si le chargement est en cours.
    /// </summary>
    bool IsLoading { get; }

    /// <summary>
    /// Le message d'erreur à afficher, s'il y en a un.
    /// </summary>
    string? ErrorMessage { get; }

    /// <summary>
    /// Charge tous les articles.
    /// </summary>
    Task LoadArticlesAsync();
}

