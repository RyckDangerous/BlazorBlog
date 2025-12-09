using Markdig;

namespace BlazorBlog.Features.CreateArticle.Services;

/// <summary>
/// Implémentation du service de rendu Markdown utilisant Markdig.
/// </summary>
public sealed class MarkdownService : IMarkdownService
{
    private readonly MarkdownPipeline _pipeline;

    /// <summary>
    /// Initialise une nouvelle instance du service Markdown.
    /// </summary>
    public MarkdownService()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
    }

    /// <inheritdoc/>
    public string ToHtml(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return string.Empty;
        }

        return Markdown.ToHtml(markdown, _pipeline);
    }
}

