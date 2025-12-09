namespace BlazorBlog.Features.Admin.Services;

/// <summary>
/// Interface pour le service de vue de la page d'administration.
/// </summary>
public interface IAdminPageViewService
{
    /// <summary>
    /// Le nombre total d'articles.
    /// </summary>
    int ArticleCount { get; }

    /// <summary>
    /// Indique si le chargement est en cours.
    /// </summary>
    bool IsLoading { get; }

    /// <summary>
    /// Le message d'erreur à afficher, s'il y en a un.
    /// </summary>
    string? ErrorMessage { get; }

    /// <summary>
    /// Charge les statistiques de l'administration.
    /// </summary>
    Task LoadStatisticsAsync();
}

