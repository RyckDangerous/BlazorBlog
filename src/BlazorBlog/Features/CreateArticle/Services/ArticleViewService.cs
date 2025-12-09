using BlazorBlog.Core.Models;
using BlazorBlog.Core.Models.Converters;
using BlazorBlog.Core.Models.ViewModels;
using BlazorBlog.Features.CreateArticle.Models;
using BlazorBlog.Features.CreateArticle.Repository;

namespace BlazorBlog.Features.CreateArticle.Services;

/// <summary>
/// Implémentation du service de vue pour les articles.
/// </summary>
public sealed class ArticleViewService : IArticleViewService
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
    public async Task<ResultOf<ArticleViewModel>> CreateArticleAsync(CreateArticleInputModel inputModel, string author, bool isPublished, CancellationToken cancellationToken = default)
    {
        // Générer le texte de prévisualisation depuis le début du contenu
        string previewText = GeneratePreviewText(inputModel.Content);

        ArticleDb articleDb = new()
        {
            Title = inputModel.Title,
            Author = author,
            Content = inputModel.Content,
            PreviewText = previewText,
            IsPublished = isPublished
        };

        ResultOf<ArticleDb> result = await _articleRepository.CreateAsync(articleDb, cancellationToken);

        if (result.IsSuccess && result.Value is not null)
        {
            return ResultOf.Success(ArticleConverter.ToViewModel(result.Value));
        }

        return ResultOf.Failure<ArticleViewModel>(result.Error!);
    }

    /// <summary>
    /// Génère le texte de prévisualisation depuis le début du contenu.
    /// </summary>
    /// <param name="content">Le contenu de l'article.</param>
    /// <returns>Le texte de prévisualisation (maximum 500 caractères).</returns>
    private static string GeneratePreviewText(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return string.Empty;
        }

        // Supprimer les retours à la ligne et les espaces multiples
        string cleaned = System.Text.RegularExpressions.Regex.Replace(content, @"\s+", " ").Trim();

        // Prendre les 500 premiers caractères
        if (cleaned.Length <= 500)
        {
            return cleaned;
        }

        // Tronquer à 500 caractères en coupant au dernier espace pour éviter de couper un mot
        string truncated = cleaned[..500];
        int lastSpace = truncated.LastIndexOf(' ');
        
        if (lastSpace > 0)
        {
            truncated = truncated[..lastSpace];
        }

        return truncated + "...";
    }
}

