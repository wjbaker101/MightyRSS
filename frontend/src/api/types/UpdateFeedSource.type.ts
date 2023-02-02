import { IApiFeedSource } from '../_shared/ApiFeedSource.type';

export interface IUpdateFeedSourceRequest {
    title: string | null;
    collection: string | null;
}

export interface IUpdateFeedSourceResponse {
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