export interface AddFeedSourceRequest {
    url: string;
}

export interface AddFeedSourceResponse {
    reference: string;
    title: string;
    description: string;
    rssUrl: string;
    websiteUrl: string;
    collection: string;
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
