import { IApiFeedSource } from '../_shared/ApiFeedSource.type';
import { IApiCollection } from '../_shared/ApiCollection.type';

export interface IGetCollectionsResponse {
    feedSourceCount: number;
    collections: Array<{
        collection: IApiCollection | null;
        feedSources: Array<IApiFeedSource>;
    }>;
}