import { IFeedSource } from '@/model/FeedSource.type';
import { IUserFeedSource } from '@/model/UserFeedSource.model';

export interface IGetConfigurationDto {
    collections: Array<{
        collection: string | null;
        feedSources: Array<{
            feedSource: IFeedSource;
            userFeedSource: IUserFeedSource;
        }>;
    }>;
}