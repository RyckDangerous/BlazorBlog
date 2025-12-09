using BlazorBlog.Core.Configurations;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Models.Errors;
using MongoDB.Driver;

namespace BlazorBlog.Features.CreateArticle.Repository;

/// <summary>
/// Implémentation du repository pour les articles MongoDB.
/// </summary>
public class ArticleRepository : IArticleRepository
{
    private readonly IMongoCollection<ArticleDb> _collection;

    /// <summary>
    /// Initialise une nouvelle instance du repository d'articles.
    /// </summary>
    /// <param name="settings">Les paramètres de configuration de l'application.</param>
    /// <param name="mongoClient">Le client MongoDB.</param>
    public ArticleRepository(IApplicationSettings settings, IMongoClient mongoClient)
    {
        IMongoDatabase database = mongoClient.GetDatabase(settings.MongoDatabaseName);
        _collection = database.GetCollection<ArticleDb>("articles");
    }

    /// <inheritdoc/>
    public async Task<ResultOf<ArticleDb>> CreateAsync(ArticleDb article, CancellationToken cancellationToken = default)
    {
        try
        {
            article.CreatedAt = DateTime.UtcNow;
            article.ModifiedAt = DateTime.UtcNow;

            await _collection.InsertOneAsync(article, cancellationToken: cancellationToken);
            return ResultOf.Success(article);
        }
        catch (Exception ex)
        {
            return ResultOf.Failure<ArticleDb>(new GenericError(ex.Message, ex));
        }
    }

    /// <inheritdoc/>
    public async Task<ResultOf<ArticleDb>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            ArticleDb? article = await _collection.Find(a => a.Id == id).FirstOrDefaultAsync(cancellationToken);
            
            if (article is null)
            {
                return ResultOf.Failure<ArticleDb>(new NotFoundError($"Article avec l'id {id} introuvable"));
            }

            return ResultOf.Success(article);
        }
        catch (Exception ex)
        {
            return ResultOf.Failure<ArticleDb>(new GenericError(ex.Message, ex));
        }
    }

    /// <inheritdoc/>
    public async Task<ResultOf<List<ArticleDb>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            List<ArticleDb> articles = await _collection.Find(_ => true).ToListAsync(cancellationToken);
            return ResultOf.Success(articles);
        }
        catch (Exception ex)
        {
            return ResultOf.Failure<List<ArticleDb>>(new GenericError(ex.Message, ex));
        }
    }

    /// <inheritdoc/>
    public async Task<ResultOf<List<ArticleDb>>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            List<ArticleDb> articles = await _collection.Find(a => a.IsPublished).ToListAsync(cancellationToken);
            return ResultOf.Success(articles);
        }
        catch (Exception ex)
        {
            return ResultOf.Failure<List<ArticleDb>>(new GenericError(ex.Message, ex));
        }
    }

    /// <inheritdoc/>
    public async Task<ResultOf<List<ArticleDb>>> GetByAuthorAsync(string author, CancellationToken cancellationToken = default)
    {
        try
        {
            List<ArticleDb> articles = await _collection.Find(a => a.Author == author).ToListAsync(cancellationToken);
            return ResultOf.Success(articles);
        }
        catch (Exception ex)
        {
            return ResultOf.Failure<List<ArticleDb>>(new GenericError(ex.Message, ex));
        }
    }
}

