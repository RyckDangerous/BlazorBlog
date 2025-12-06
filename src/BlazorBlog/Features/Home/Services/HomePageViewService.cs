using BlazorBlog.Core.Models;
using BlazorBlog.Features.CreateArticle.Repository;

namespace BlazorBlog.Features.Home.Services;

/// <summary>
/// Service de vue pour la page d'accueil.
/// Gère l'état et la logique de présentation de la page.
/// </summary>
public sealed class HomePageViewService : IHomePageViewService
{
    private readonly IArticleRepository _articleRepository;

    /// <summary>
    /// Initialise une nouvelle instance du service de vue pour la page d'accueil.
    /// </summary>
    /// <param name="articleRepository">Le repository des articles.</param>
    public HomePageViewService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    /// <inheritdoc/>
    public List<Article> Articles { get; private set; } = [];

    /// <inheritdoc/>
    public bool IsLoading { get; private set; }

    /// <inheritdoc/>
    public string? ErrorMessage { get; private set; }

    /// <inheritdoc/>
    public async Task LoadArticlesAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            ResultOf<List<Article>> result = await _articleRepository.GetAllAsync();

            if (result.IsSuccess && result.Value is not null)
            {
                // Trier par date de création décroissante
                Articles = result.Value.OrderByDescending(a => a.CreatedAt).ToList();
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors du chargement des articles.";
                Articles = [];
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur : {ex.Message}";
            Articles = [];
        }
        finally
        {
            IsLoading = false;
        }
    }
}

