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
    Task<ResultOf<Article>> CreateAsync(Article article, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère un article par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'article.</param>
    /// <param name="cancellationToken">Token d'annulation.</param>
    /// <returns>Le résultat de l'opération avec l'article trouvé.</returns>
    Task<ResultOf<Article>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère tous les articles.
    /// </summary>
    /// <param name="cancellationToken">Token d'annulation.</param>
    /// <returns>Le résultat de l'opération avec la liste des articles.</returns>
    Task<ResultOf<List<Article>>> GetAllAsync(CancellationToken cancellationToken = default);
}

