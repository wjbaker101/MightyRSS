using System.ServiceModel.Syndication;
using System.Xml;

namespace Api.Tests;

[TestFixture]
[Parallelizable]
public sealed class UnitTest1
{
    [OneTimeSetUp]
    public async Task Setup()
    {
        const string url = "http://www.sciencedaily.com/rss/health_medicine/nutrition.xml";

        //var test = await FeedReader.ReadAsync(url);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.3");

        var response = await new HttpClient().SendAsync(httpRequestMessage);

        var reader = XmlReader.Create(await response.Content.ReadAsStreamAsync());
        var feed = SyndicationFeed.Load(reader);
        reader.Close();
    }

    [Test]
    public void Then()
    {
    }
}