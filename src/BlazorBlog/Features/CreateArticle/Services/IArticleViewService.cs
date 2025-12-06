using BlazorBlog.Core.Models;
using BlazorBlog.Features.CreateArticle.Models;

namespace BlazorBlog.Features.CreateArticle.Services;

/// <summary>
/// Interface pour le service de vue des articles.
/// </summary>
public interface IArticleViewService
{
    /// <summary>
    /// Crée un nouvel article à partir d'un modèle d'entrée.
    /// </summary>
    /// <param name="inputModel">Le modèle d'entrée contenant les données de l'article.</param>
    /// <param name="cancellationToken">Token d'annulation.</param>
    /// <returns>Le résultat de l'opération avec l'article créé.</returns>
    Task<ResultOf<Article>> CreateArticleAsync(CreateArticleInputModel inputModel, CancellationToken cancellationToken = default);
}

