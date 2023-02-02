import axios, { AxiosRequestConfig } from 'axios';
import dayjs from 'dayjs';

import { useAppData } from '@/use/app-data.use';
import { ApiResultResponse, responseHelper } from './ResponseHelper';

import { IUser } from '@/model/User.model';
import { IGetSelfResponse } from './types/GetSelf.type';
import { IGetCollectionsResponse } from './types/GetCollections.type';
import { IGetCollectionsDto } from './dtos/GetCollections.dto';
import { collectionMapper } from './_shared/mapper/collection.mapper';
import { feedSourceMapper } from './_shared/mapper/feed-source.mapper';
import { ICollection } from '@/model/Collection.model';
import { ICreateCollectionRequest, ICreateCollectionResponse } from './types/CreateCollection.type';
import { IUpdateCollectionRequest, IUpdateCollectionResponse } from './types/UpdateCollection.type';
import { GetFeedResponse } from './feed/types/GetFeed.type';
import { AddFeedSourceRequest, AddFeedSourceResponse } from './feed/types/AddFeedSource.type';
import { UpdateFeedSourceRequest } from './feed/types/UpdateFeedSource.type';

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

        async getSelf(): Promise<IUser | Error> {
            try {
                const response = await api.get<ApiResultResponse<IGetSelfResponse>>('/users');

                const user = response.data.result.user;

                return {
                    reference: user.reference,
                    createdAt: dayjs(user.createdAt),
                    username: user.username,
                };
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

    },

    collections: {

        async get(): Promise<IGetCollectionsDto> {
            const response = await api.get<ApiResultResponse<IGetCollectionsResponse>>('/collections');

            const result = response.data.result;

            return {
                feedSourceCount: result.feedSourceCount,
                collections: result.collections.map(x => ({
                    collection: x.collection === null ? null : collectionMapper.map(x.collection),
                    feedSources: x.feedSources.map(feedSourceMapper.map),
                })),
            };
        },

        async add(request: ICreateCollectionRequest): Promise<ICollection> {
            const response = await api.post<ApiResultResponse<ICreateCollectionResponse>>('/collections', request);

            const collection = response.data.result.collection;

            return {
                reference: collection.reference,
                createdAt: dayjs(collection.createdAt),
                name: collection.name,
            };
        },

        async update(collectionReference: string, request: IUpdateCollectionRequest): Promise<ICollection> {
            const response = await api.put<ApiResultResponse<IUpdateCollectionResponse>>(`/collections/${collectionReference}`, request);

            const collection = response.data.result.collection;

            return {
                reference: collection.reference,
                createdAt: dayjs(collection.createdAt),
                name: collection.name,
            };
        },

    },

    feed: {

        async get(): Promise<GetFeedResponse | Error> {
            try {
                const response = await api.get<ApiResultResponse<GetFeedResponse>>('/feed');

                return response.data.result;
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

        async addSource(request: AddFeedSourceRequest): Promise<AddFeedSourceResponse | Error> {
            try {
                const response = await api.post<ApiResultResponse<AddFeedSourceResponse>>('/feed', request);

                return response.data.result;
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

        async updateSource(reference: string, request: UpdateFeedSourceRequest): Promise<AddFeedSourceResponse | Error> {
            try {
                const response = await api.put<ApiResultResponse<AddFeedSourceResponse>>(`/feed/source/${reference}`, request);

                return response.data.result;
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

        async deleteSource(reference: string): Promise<void | Error> {
            try {
                await api.delete<ApiResultResponse<AddFeedSourceResponse>>(`/feed/source/${reference}`);
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

    },

};