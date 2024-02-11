using Data.Records;
using Data.UoW;
using MightyRSS.Api.FeedSources.Types;
using MightyRSS.Models.Mappers;
using NetApiLibs.Type;

namespace MightyRSS.Api.FeedSources;

public interface IFeedSourcesService
{
    Task<Result<AddFeedSourceResponse>> AddFeedSource(UserRecord user, AddFeedSourceRequest request, CancellationToken cancellationToken);
    Task<Result<UpdateFeedSourceResponse>> UpdateFeedSource(UserRecord user, Guid feedReference, UpdateFeedSourceRequest request, CancellationToken cancellationToken);
    Task<Result<DeleteFeedSourceResponse>> DeleteFeedSource(UserRecord user, Guid reference, CancellationToken cancellationToken);
}

public sealed class FeedSourcesService : IFeedSourcesService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;
    private readonly IFeedReaderService _feedReaderService;

    public FeedSourcesService(IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory, IFeedReaderService feedReaderService)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
        _feedReaderService = feedReaderService;
    }

    public async Task<Result<AddFeedSourceResponse>> AddFeedSource(UserRecord user, AddFeedSourceRequest request, CancellationToken cancellationToken)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create(cancellationToken);

        var feedSourceResult = await unitOfWork.FeedSources.GetByRssUrl(request.Url);
        if (!feedSourceResult.TrySuccess(out var feedSource))
        {
            var feedDetailsResult = await _feedReaderService.Read(request.Url, null);
            if (feedDetailsResult.IsFailure)
                return Result<AddFeedSourceResponse>.FromFailure(feedDetailsResult);

            var feedDetails = feedDetailsResult.Value;

            feedSource = new FeedSourceRecord
            {
                Reference = feedDetails.Reference,
                Title = feedDetails.Title,
                Description = feedDetails.Description,
                RssUrl = feedDetails.RssUrl,
                WebsiteUrl = feedDetails.WebsiteUrl,
                Articles = feedDetails.Articles.ConvertAll(x => new FeedSourceRecord.Article
                {
                    Url = x.Url,
                    Title = x.Title,
                    Summary = x.Summary,
                    PublishedAt = x.PublishedAt,
                    PublishedAtAsString = x.PublishedAtAsString,
                    Author = x.Author
                }),
                ArticlesUpdatedAt = DateTime.UtcNow
            };

            await unitOfWork.FeedSources.Save(feedSource);
        }

        var existingUserFeedSource = await unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, feedSource.Reference);
        if (existingUserFeedSource.IsSuccess)
            return Result<AddFeedSourceResponse>.Failure("Feed source could not be added as it already exists.");

        var userFeedSource = new UserFeedSourceRecord
        {
            User = user,
            FeedSource = feedSource,
            Collection = null,
            Title = feedSource.Title,
            CollectionRecord = null
        };

        await unitOfWork.UserFeedSources.Save(userFeedSource);

        await unitOfWork.Commit();

        return new AddFeedSourceResponse
        {
            FeedSource = FeedSourceMapper.Map(feedSource, userFeedSource),
            Articles = feedSource.Articles.ConvertAll(x => new AddFeedSourceResponse.FeedArticle
            {
                Url = x.Url,
                Title = x.Title,
                Summary = x.Summary,
                Author = x.Author,
                PublishedAt = x.PublishedAt,
                PublishedAtAsString = x.PublishedAtAsString
            })
        };
    }

    public async Task<Result<UpdateFeedSourceResponse>> UpdateFeedSource(UserRecord user, Guid feedReference, UpdateFeedSourceRequest request, CancellationToken cancellationToken)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create(cancellationToken);

        var userFeedSourceResult = await unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, feedReference);
        if (!userFeedSourceResult.TrySuccess(out var userFeedSource))
            return Result<UpdateFeedSourceResponse>.FromFailure(userFeedSourceResult);

        userFeedSource.Collection = request.Collection;
        userFeedSource.Title = request.Title;

        await unitOfWork.UserFeedSources.Update(userFeedSource);

        await unitOfWork.Commit();

        return new UpdateFeedSourceResponse();
    }

    public async Task<Result<DeleteFeedSourceResponse>> DeleteFeedSource(UserRecord user, Guid reference, CancellationToken cancellationToken)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create(cancellationToken);

        var userFeedSourceResult = await unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, reference);
        if (!userFeedSourceResult.TrySuccess(out var userFeedSource))
            return Result<DeleteFeedSourceResponse>.FromFailure(userFeedSourceResult);

        await unitOfWork.UserFeedSources.Delete(userFeedSource);

        await unitOfWork.Commit();

        return new DeleteFeedSourceResponse();
    }
}