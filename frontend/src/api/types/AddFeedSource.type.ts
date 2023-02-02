import { IApiFeedSource } from '@/api/_shared/ApiFeedSource.type';

export interface IAddFeedSourceRequest {
    readonly url: string;
}

export interface IAddFeedSourceResponse {
    readonly feedSource: IApiFeedSource;
    readonly articles: Array<IFeedArticle>;
}

export interface IFeedArticle {
    readonly url: string;
    readonly title: string;
    readonly summary: string;
    readonly author: string;
    readonly publishedAt: string;
    readonly publishedAtAsString: string;
}