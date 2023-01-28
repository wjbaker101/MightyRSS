import { IApiFeedSource } from '@/api/_shared/ApiFeedSource.type';
import { IApiUserFeedSource } from '@/api/_shared/ApiUserFeedSource.type';

export interface IGetConfigurationResponse {
    collections: Array<{
        collection: string | null;
        feedSources: Array<{
            feedSource: IApiFeedSource;
            userFeedSource: IApiUserFeedSource;
        }>;
    }>;
}