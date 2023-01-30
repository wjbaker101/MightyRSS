import { IApiFeedSource } from '@/api/_shared/ApiFeedSource.type';

export interface GetFeedResponse {
    readonly sources: Array<FeedSource>;
}

export interface FeedSource {
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