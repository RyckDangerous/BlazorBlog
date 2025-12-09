using BlazorBlog.Features.CreateArticle.Repository;

namespace BlazorBlog.Features.Admin.Services;

/// <summary>
/// Service de vue pour la page d'administration.
/// Gère l'état et la logique de présentation de la page.
/// </summary>
public sealed class AdminPageViewService : IAdminPageViewService
{
    private readonly IArticleRepository _articleRepository;

    /// <summary>
    /// Initialise une nouvelle instance du service de vue pour la page d'administration.
    /// </summary>
    /// <param name="articleRepository">Le repository des articles.</param>
    public AdminPageViewService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    /// <inheritdoc/>
    public int ArticleCount { get; private set; }

    /// <inheritdoc/>
    public bool IsLoading { get; private set; }

    /// <inheritdoc/>
    public string? ErrorMessage { get; private set; }

    /// <inheritdoc/>
    public async Task LoadStatisticsAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var result = await _articleRepository.GetAllAsync();

            if (result.IsSuccess && result.Value is not null)
            {
                ArticleCount = result.Value.Count;
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors du chargement des statistiques.";
                ArticleCount = 0;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur : {ex.Message}";
            ArticleCount = 0;
        }
        finally
        {
            IsLoading = false;
        }
    }
}

