using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Features.CreateArticle.Models;

/// <summary>
/// Modèle d'entrée pour la création d'un article.
/// </summary>
public sealed record CreateArticleInputModel
{
    /// <summary>
    /// Le titre de l'article.
    /// </summary>
    [Required(ErrorMessage = "Le titre est requis")]
    [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Le nom de l'auteur de l'article.
    /// </summary>
    [Required(ErrorMessage = "L'auteur est requis")]
    [StringLength(100, ErrorMessage = "Le nom d'auteur ne peut pas dépasser 100 caractères")]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Le contenu de l'article en Markdown.
    /// </summary>
    [Required(ErrorMessage = "Le contenu est requis")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Le texte de prévisualisation affiché sur les cartes d'articles.
    /// </summary>
    [Required(ErrorMessage = "Le texte de prévisualisation est requis")]
    [StringLength(500, ErrorMessage = "Le texte de prévisualisation ne peut pas dépasser 500 caractères")]
    public string PreviewText { get; set; } = string.Empty;
}

