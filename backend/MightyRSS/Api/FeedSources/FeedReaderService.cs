using MightyRSS.Api.FeedSources.Types;
using NetApiLibs.Extension;
using NetApiLibs.Type;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MightyRSS.Api.FeedSources;

public interface IFeedReaderService
{
    Task<Result<FeedDetails>> Read(string url, Guid? reference);
}

public sealed class FeedReaderService : IFeedReaderService
{
    private static readonly HttpClient _httpClient = new();

    public async Task<Result<FeedDetails>> Read(string url, Guid? reference)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var sourceUrl))
            return Result<FeedDetails>.Failure("The given URL was formatted incorrectly please try again.");

        try
        {
            return await Read(sourceUrl.ToString(), url, reference);
        }
        catch
        {
            return Result<FeedDetails>.Failure("Sorry, unable to retrieve details of the feed. Please try again later.");
        }
    }

    private static async Task<FeedDetails> Read(string sourceUrl, string url, Guid? reference)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, sourceUrl);
        httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.3");

        var response = await _httpClient.SendAsync(httpRequestMessage);

        var reader = XmlReader.Create(await response.Content.ReadAsStreamAsync());
        var feed = SyndicationFeed.Load(reader);
        reader.Close();

        return new FeedDetails
        {
            Reference = reference ?? Guid.NewGuid(),
            Title = feed.Title.Text,
            Description = feed.Description.Text,
            RssUrl = url,
            WebsiteUrl = feed.Links.FirstOrDefault()?.Uri.ToString() ?? "Unknown Website",
            Articles = feed.Items.ConvertAll(x => new FeedDetails.Article
            {
                Url = x.Links.FirstOrDefault()?.Uri.ToString() ?? "Unknown Post Link",
                Title = x.Title.Text,
                Summary = x.Summary.Text,
                Author = x.Authors.FirstOrDefault()?.Name ?? "Unknown Author",
                PublishedAt = x.PublishDate.DateTime,
                PublishedAtAsString = x.PublishDate.DateTime.ToLongDateString()
            })
        };
    }
}