import { IApiFeedSource } from '@/api/_shared/ApiFeedSource.type';

export interface AddFeedSourceRequest {
    readonly url: string;
}

export interface AddFeedSourceResponse {
    readonly feedSource: IApiFeedSource;
    readonly articles: Array<FeedArticle>;
}

export interface FeedArticle {
    readonly url: string;
    readonly title: string;
    readonly summary: string;
    readonly author: string;
    readonly publishedAt: string;
    readonly publishedAtAsString: string;
}