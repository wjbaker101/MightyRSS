import axios, { AxiosRequestConfig } from 'axios';
import dayjs from 'dayjs';

import { useAppData } from '@/use/app-data.use';
import { ApiResultResponse } from './ResponseHelper';

import { IGetConfigurationDto } from './dtos/GetConfiguration.dto';
import { IGetConfigurationResponse } from './types/GetConfiguration.type';

import { IUser } from '@/model/User.model';
import { IGetSelfResponse } from './types/GetSelf.type';
import { ICollection } from '@/model/Collection.model';
import { IGetCollectionsResponse } from './types/GetCollections.type';
import { IGetCollectionsDto } from './dtos/GetCollections.dto';
import { collectionMapper } from './_shared/mapper/collection.mapper';
import { IFeedSource } from '@/model/FeedSource.model';
import { feedSourceMapper } from './_shared/mapper/feed-source.mapper';

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

    collections: {

        async get(): Promise<IGetCollectionsDto> {
            const response = await api.get<ApiResultResponse<IGetCollectionsResponse>>('/collections');

            const collections = response.data.result.collections;

            return {
                collections: collections.map(x => ({
                    collection: x.collection === null ? null : collectionMapper.map(x.collection),
                    feedSources: x.feedSources.map(feedSourceMapper.map),
                })),
            };
        },

    },

};