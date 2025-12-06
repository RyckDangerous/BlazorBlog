using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorBlog.Core.Models;

/// <summary>
/// Représente un article de blog stocké dans MongoDB.
/// </summary>
public sealed record Article
{
    /// <summary>
    /// L'identifiant unique de l'article (ObjectId MongoDB).
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Le titre de l'article.
    /// </summary>
    [BsonElement("title")]
    public required string Title { get; set; }

    /// <summary>
    /// Le nom de l'auteur de l'article.
    /// </summary>
    [BsonElement("author")]
    public required string Author { get; set; }

    /// <summary>
    /// Le contenu de l'article en Markdown.
    /// </summary>
    [BsonElement("content")]
    public required string Content { get; set; }

    /// <summary>
    /// Le texte de prévisualisation affiché sur les cartes d'articles.
    /// </summary>
    [BsonElement("previewText")]
    public required string PreviewText { get; set; }

    /// <summary>
    /// La date et l'heure de création de l'article (UTC).
    /// </summary>
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// La date et l'heure de dernière modification de l'article (UTC).
    /// </summary>
    [BsonElement("modifiedAt")]
    public DateTime ModifiedAt { get; set; }
}

