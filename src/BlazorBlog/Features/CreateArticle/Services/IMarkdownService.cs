namespace BlazorBlog.Features.CreateArticle.Services;

/// <summary>
/// Service pour le rendu et la manipulation de contenu Markdown.
/// </summary>
public interface IMarkdownService
{
    /// <summary>
    /// Convertit du contenu Markdown en HTML.
    /// </summary>
    /// <param name="markdown">Le contenu Markdown à convertir.</param>
    /// <returns>Le HTML généré à partir du Markdown.</returns>
    string ToHtml(string markdown);
}

