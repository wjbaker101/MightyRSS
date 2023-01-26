import axios, { AxiosRequestConfig } from 'axios';

import { useAppData } from '@/use/app-data.use';
import { IGetConfigurationDto } from './dtos/GetConfiguration.dto';
import { IGetConfigurationResponse } from './types/GetConfiguration.type';
import { ApiResultResponse } from '../ResponseHelper';

const appData = useAppData();

const api = axios.create({
    baseURL: '/api/configuration',
});

api.interceptors.request.use((config: AxiosRequestConfig) => {
    config.headers = {
        'Authorisation': appData.auth.loginToken.value,
    };

    return config;
});

export const configurationApi = {

    async getConfiguration(): Promise<IGetConfigurationDto> {
        const response = await api.get<ApiResultResponse<IGetConfigurationResponse>>('');

        const collections = response.data.result.collections;

        return {
            collections: collections.map(collection => ({
                collection: collection.collection,
                feedSources: collection.feedSources.map(({ feedSource, userFeedSource }) => ({
                    feedSource: {
                        reference: feedSource.reference,
                        title: feedSource.title,
                        description: feedSource.description,
                        rssUrl: feedSource.rssUrl,
                        websiteUrl: feedSource.websiteUrl,
                        collection: feedSource.collection,
                        titleAlias: feedSource.titleAlias,
                    },
                    userFeedSource: {
                        collection: userFeedSource.collection,
                        titleAlias: userFeedSource.titleAlias,
                    },
                })),
            })),
        };
    },

};