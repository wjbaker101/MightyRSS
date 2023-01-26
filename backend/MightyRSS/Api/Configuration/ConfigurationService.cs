using Core.Models.Mappers;
using Data.UoW;
using MightyRSS.Api.Configuration.Types;
using MightyRSS.Types;
using NetApiLibs.Extension;
using NetApiLibs.Type;
using System.Linq;

namespace MightyRSS.Api.Configuration;

public interface IConfigurationService
{
    Result<GetConfigurationResponse> GetConfiguration(IRequestContext requestContext);
}

public sealed class ConfigurationService : IConfigurationService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;

    public ConfigurationService(IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
    }

    public Result<GetConfigurationResponse> GetConfiguration(IRequestContext requestContext)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var feedSources = unitOfWork.UserFeedSources.GetFeedSources(requestContext.User);

        var groupedByCollection = feedSources.GroupBy(x => x.Collection);

        return new GetConfigurationResponse
        {
            Collections = groupedByCollection.ConvertAll(collection => new GetConfigurationResponse.FeedSourceCollection
            {
                Collection = collection.Key,
                FeedSources = collection.ConvertAll(userFeedSource => new GetConfigurationResponse.FeedSourceDetails
                {
                    FeedSource = FeedSourceMapper.Map(userFeedSource.FeedSource),
                    UserFeedSource = UserFeedSourceMapper.Map(userFeedSource)
                })
            })
        };
    }
}