using BlazorBlog.Features.CreateArticle.Models;

namespace BlazorBlog.Features.CreateArticle.Services;

/// <summary>
/// Interface pour le service de vue de la page de création d'article.
/// </summary>
public interface ICreateArticlePageViewService
{
    /// <summary>
    /// Le modèle d'entrée pour le formulaire.
    /// </summary>
    CreateArticleInputModel InputModel { get; }

    /// <summary>
    /// Indique si une soumission est en cours.
    /// </summary>
    bool IsSubmitting { get; }

    /// <summary>
    /// Le message d'erreur à afficher, s'il y en a un.
    /// </summary>
    string? ErrorMessage { get; }

    /// <summary>
    /// Initialise le service de vue.
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Sauvegarde l'article en brouillon (non publié).
    /// </summary>
    /// <returns>True si la sauvegarde a réussi, false sinon.</returns>
    Task<bool> SaveAsDraftAsync();

    /// <summary>
    /// Publie l'article (visible publiquement).
    /// </summary>
    /// <returns>True si la publication a réussi, false sinon.</returns>
    Task<bool> PublishAsync();

    /// <summary>
    /// Annule la création et redirige vers la page d'accueil.
    /// </summary>
    void Cancel();
}

