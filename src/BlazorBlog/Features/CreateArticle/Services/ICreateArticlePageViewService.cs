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
    /// Initialise le modèle d'entrée avec le nom de l'utilisateur connecté.
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Gère la soumission du formulaire de création d'article.
    /// </summary>
    /// <returns>True si la création a réussi, false sinon.</returns>
    Task<bool> HandleSubmitAsync();

    /// <summary>
    /// Annule la création et redirige vers la page d'accueil.
    /// </summary>
    void Cancel();
}

