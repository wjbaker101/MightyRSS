export interface GetFeedResponse {
    sources: Array<FeedSource>;
}

export interface FeedSource {
    reference: string;
    title: string;
    description: string;
    rssUrl: string;
    websiteUrl: string;
    articles: Array<FeedArticle>;
}

export interface FeedArticle {
    url: string;
    title: string;
    summary: string;
    author: string;
    publishedAt: string;
    publishedAtAsString: string;
}
