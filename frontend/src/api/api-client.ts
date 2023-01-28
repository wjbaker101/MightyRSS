import axios, { AxiosRequestConfig } from 'axios';
import dayjs from 'dayjs';

import { useAppData } from '@/use/app-data.use';
import { ApiResultResponse } from './ResponseHelper';

import { IGetConfigurationDto } from './dtos/GetConfiguration.dto';
import { IGetConfigurationResponse } from './types/GetConfiguration.type';

import { IUser } from '@/model/User.model';
import { IGetSelfResponse } from './types/GetSelf.type';

const appData = useAppData();

const api = axios.create({
    baseURL: '/api',
});

api.interceptors.request.use((config: AxiosRequestConfig) => {
    config.headers = {
        'Authorisation': appData.auth.loginToken.value,
    };

    return config;
});

export const apiClient = {

    configuration: {

        async get(): Promise<IGetConfigurationDto> {
            const response = await api.get<ApiResultResponse<IGetConfigurationResponse>>('/configuration');

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

    },

    user: {

        async getSelf(): Promise<IUser> {
            const response = await api.get<ApiResultResponse<IGetSelfResponse>>('/users');

            const user = response.data.result.user;

            return {
                reference: user.reference,
                createdAt: dayjs(user.createdAt),
                username: user.username,
            };
        },

    },

};