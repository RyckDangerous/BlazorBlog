using BlazorBlog.Core.Models;
using BlazorBlog.Core.Models.ViewModels;
using BlazorBlog.Features.CreateArticle.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorBlog.Features.CreateArticle.Services;

/// <summary>
/// Service de vue pour la page de création d'article.
/// Gère l'état et la logique de présentation de la page.
/// </summary>
public sealed class CreateArticlePageViewService : ICreateArticlePageViewService
{
    private readonly IArticleViewService _articleViewService;
    private readonly NavigationManager _navigationManager;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    /// <summary>
    /// Initialise une nouvelle instance du service de vue pour la page de création d'article.
    /// </summary>
    /// <param name="articleViewService">Le service de vue des articles.</param>
    /// <param name="navigationManager">Le gestionnaire de navigation.</param>
    /// <param name="authenticationStateProvider">Le fournisseur d'état d'authentification.</param>
    public CreateArticlePageViewService(
        IArticleViewService articleViewService,
        NavigationManager navigationManager,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _articleViewService = articleViewService;
        _navigationManager = navigationManager;
        _authenticationStateProvider = authenticationStateProvider;
    }

    /// <inheritdoc/>
    public CreateArticleInputModel InputModel { get; private set; } = new();

    /// <inheritdoc/>
    public bool IsSubmitting { get; private set; }

    /// <inheritdoc/>
    public string? ErrorMessage { get; private set; }

    /// <inheritdoc/>
    public Task InitializeAsync()
    {
        // Plus besoin d'initialiser l'auteur, il sera passé lors de la création
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<bool> SaveAsDraftAsync()
    {
        return await SaveArticleAsync(isPublished: false);
    }

    /// <inheritdoc/>
    public async Task<bool> PublishAsync()
    {
        return await SaveArticleAsync(isPublished: true);
    }

    /// <summary>
    /// Sauvegarde l'article avec le statut de publication spécifié.
    /// </summary>
    /// <param name="isPublished">Indique si l'article doit être publié.</param>
    /// <returns>True si la sauvegarde a réussi, false sinon.</returns>
    private async Task<bool> SaveArticleAsync(bool isPublished)
    {
        IsSubmitting = true;
        ErrorMessage = null;

        try
        {
            // Récupérer l'auteur depuis l'utilisateur connecté
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            string author = authState.User?.Identity?.Name ?? "Anonyme";

            ResultOf<ArticleViewModel> result = await _articleViewService.CreateArticleAsync(InputModel, author, isPublished);

            if (result.IsSuccess && result.Value is not null)
            {
                if (isPublished)
                {
                    _navigationManager.NavigateTo($"/articles/{result.Value.Id}");
                }
                else
                {
                    // Rediriger vers l'admin après sauvegarde en brouillon
                    _navigationManager.NavigateTo("/admin");
                }
                return true;
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la sauvegarde de l'article.";
                return false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur : {ex.Message}";
            return false;
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    /// <inheritdoc/>
    public void Cancel()
    {
        _navigationManager.NavigateTo("/");
    }
}

