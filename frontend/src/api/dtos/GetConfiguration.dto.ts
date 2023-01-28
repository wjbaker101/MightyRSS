import { IFeedSource } from '@/model/FeedSource.model';
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