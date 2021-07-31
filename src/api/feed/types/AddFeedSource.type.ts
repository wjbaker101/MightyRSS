export interface AddFeedSourceRequest {
    url: string;
}

export interface AddFeedSourceResponse {
    reference: string;
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
}
