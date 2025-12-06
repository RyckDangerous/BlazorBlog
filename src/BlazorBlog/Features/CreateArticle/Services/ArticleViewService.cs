using BlazorBlog.Core.Models;
using BlazorBlog.Features.CreateArticle.Models;
using BlazorBlog.Features.CreateArticle.Repository;

namespace BlazorBlog.Features.CreateArticle.Services;

/// <summary>
/// Implémentation du service de vue pour les articles.
/// </summary>
public class ArticleViewService : IArticleViewService
{
    private readonly IArticleRepository _articleRepository;

    /// <summary>
    /// Initialise une nouvelle instance du service de vue des articles.
    /// </summary>
    /// <param name="articleRepository">Le repository des articles.</param>
    public ArticleViewService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    /// <inheritdoc/>
    public async Task<ResultOf<Article>> CreateArticleAsync(CreateArticleInputModel inputModel, CancellationToken cancellationToken = default)
    {
        Article article = new()
        {
            Title = inputModel.Title,
            Author = inputModel.Author,
            Content = inputModel.Content,
            PreviewText = inputModel.PreviewText
        };

        return await _articleRepository.CreateAsync(article, cancellationToken);
    }
}

