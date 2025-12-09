namespace BlazorBlog.Core.Models.ViewModels;

/// <summary>
/// ViewModel pour l'affichage d'un article dans les vues.
/// </summary>
public sealed record ArticleViewModel
{
    /// <summary>
    /// L'identifiant unique de l'article.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Le titre de l'article.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Le nom de l'auteur de l'article.
    /// </summary>
    public required string Author { get; init; }

    /// <summary>
    /// Le contenu de l'article en Markdown.
    /// </summary>
    public required string Content { get; init; }

    /// <summary>
    /// Le texte de prévisualisation affiché sur les cartes d'articles.
    /// </summary>
    public required string PreviewText { get; init; }

    /// <summary>
    /// La date et l'heure de création de l'article (UTC).
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// La date et l'heure de dernière modification de l'article (UTC).
    /// </summary>
    public DateTime ModifiedAt { get; init; }

    /// <summary>
    /// Indique si l'article est publié et visible publiquement.
    /// </summary>
    public bool IsPublished { get; init; }
}

