using BlazorBlog.Core.Models;

namespace BlazorBlog.Features.CreateArticle.Repository;

/// <summary>
/// Interface pour le repository des articles MongoDB.
/// </summary>
public interface IArticleRepository
{
    /// <summary>
    /// Crée un nouvel article dans MongoDB.
    /// </summary>
    /// <param name="article">L'article à créer.</param>
    /// <param name="cancellationToken">Token d'annulation.</param>
    /// <returns>Le résultat de l'opération avec l'article créé.</returns>
    Task<ResultOf<ArticleDb>> CreateAsync(ArticleDb article, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère un article par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'article.</param>
    /// <param name="cancellationToken">Token d'annulation.</param>
    /// <returns>Le résultat de l'opération avec l'article trouvé.</returns>
    Task<ResultOf<ArticleDb>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère tous les articles.
    /// </summary>
    /// <param name="cancellationToken">Token d'annulation.</param>
    /// <returns>Le résultat de l'opération avec la liste des articles.</returns>
    Task<ResultOf<List<ArticleDb>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère tous les articles publiés.
    /// </summary>
    /// <param name="cancellationToken">Token d'annulation.</param>
    /// <returns>Le résultat de l'opération avec la liste des articles publiés.</returns>
    Task<ResultOf<List<ArticleDb>>> GetPublishedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère tous les articles d'un auteur (publiés et brouillons).
    /// </summary>
    /// <param name="author">Le nom de l'auteur.</param>
    /// <param name="cancellationToken">Token d'annulation.</param>
    /// <returns>Le résultat de l'opération avec la liste des articles de l'auteur.</returns>
    Task<ResultOf<List<ArticleDb>>> GetByAuthorAsync(string author, CancellationToken cancellationToken = default);
}

