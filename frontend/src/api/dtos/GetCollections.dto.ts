import { ICollection } from '@/model/Collection.model';
import { IFeedSource } from '@/model/FeedSource.model';

export interface IGetCollectionsDto {
    feedSourceCount: number;
    collections: Array<{
        collection: ICollection | null;
        feedSources: Array<IFeedSource>;
    }>;
}