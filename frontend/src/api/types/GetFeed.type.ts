import { IApiFeedSource } from '@/api/_shared/ApiFeedSource.type';

export interface IGetFeedResponse {
    readonly sources: Array<IFeedSource>;
}

export interface IFeedSource {
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