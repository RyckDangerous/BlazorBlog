using BlazorBlog.Core.Models;
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
    public async Task InitializeAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User?.Identity?.Name is not null)
        {
            InputModel = InputModel with { Author = authState.User.Identity.Name };
        }
    }

    /// <inheritdoc/>
    public async Task<bool> HandleSubmitAsync()
    {
        IsSubmitting = true;
        ErrorMessage = null;

        try
        {
            ResultOf<Article> result = await _articleViewService.CreateArticleAsync(InputModel);

            if (result.IsSuccess && result.Value is not null)
            {
                _navigationManager.NavigateTo($"/articles/{result.Value.Id}");
                return true;
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création de l'article.";
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

