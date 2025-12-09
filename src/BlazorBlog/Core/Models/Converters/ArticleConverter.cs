using BlazorBlog.Core.Models;
using BlazorBlog.Core.Models.ViewModels;

namespace BlazorBlog.Core.Models.Converters;

/// <summary>
/// Convertisseur statique pour convertir entre ArticleDb et ArticleViewModel.
/// </summary>
public static class ArticleConverter
{
    /// <summary>
    /// Convertit un ArticleDb en ArticleViewModel.
    /// </summary>
    /// <param name="articleDb">L'article de la base de données.</param>
    /// <returns>Le ViewModel de l'article.</returns>
    public static ArticleViewModel ToViewModel(ArticleDb articleDb)
    {
        return new ArticleViewModel
        {
            Id = articleDb.Id,
            Title = articleDb.Title,
            Author = articleDb.Author,
            Content = articleDb.Content,
            PreviewText = articleDb.PreviewText,
            CreatedAt = articleDb.CreatedAt,
            ModifiedAt = articleDb.ModifiedAt,
            IsPublished = articleDb.IsPublished
        };
    }

    /// <summary>
    /// Convertit une liste d'ArticleDb en liste d'ArticleViewModel.
    /// </summary>
    /// <param name="articlesDb">La liste des articles de la base de données.</param>
    /// <returns>La liste des ViewModels des articles.</returns>
    public static List<ArticleViewModel> ToViewModels(IEnumerable<ArticleDb> articlesDb)
    {
        return articlesDb.Select(ToViewModel).ToList();
    }

    /// <summary>
    /// Convertit un ArticleViewModel en ArticleDb.
    /// </summary>
    /// <param name="viewModel">Le ViewModel de l'article.</param>
    /// <returns>L'article de la base de données.</returns>
    public static ArticleDb ToDb(ArticleViewModel viewModel)
    {
        return new ArticleDb
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            Author = viewModel.Author,
            Content = viewModel.Content,
            PreviewText = viewModel.PreviewText,
            CreatedAt = viewModel.CreatedAt,
            ModifiedAt = viewModel.ModifiedAt,
            IsPublished = viewModel.IsPublished
        };
    }
}

