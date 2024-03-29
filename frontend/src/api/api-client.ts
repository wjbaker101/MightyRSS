import axios, { AxiosRequestConfig } from 'axios';
import dayjs from 'dayjs';

import { useAuth } from '@/use/auth.use';
import { IApiResultResponse, responseHelper } from '@/api/ResponseHelper';

import { IUser } from '@/model/User.model';
import { ICollection } from '@/model/Collection.model';

import { collectionMapper } from '@/api/_shared/mapper/collection.mapper';
import { feedSourceMapper } from '@/api/_shared/mapper/feed-source.mapper';

import { IGetCollectionsDto } from '@/api/dtos/GetCollections.dto';

import { IGetSelfResponse } from '@/api/types/GetSelf.type';
import { IGetCollectionsResponse } from '@/api/types/GetCollections.type';
import { ICreateCollectionRequest, ICreateCollectionResponse } from '@/api/types/CreateCollection.type';
import { IUpdateCollectionRequest, IUpdateCollectionResponse } from '@/api/types/UpdateCollection.type';
import { IGetFeedResponse } from '@/api/types/GetFeed.type';
import { IAddFeedSourceRequest, IAddFeedSourceResponse } from '@/api/types/AddFeedSource.type';
import { IUpdateFeedSourceRequest, IUpdateFeedSourceResponse } from '@/api/types/UpdateFeedSource.type';

const auth = useAuth();

const api = axios.create({
    baseURL: '/api',
});

api.interceptors.request.use((config: AxiosRequestConfig) => {
    config.headers = {
        'Authorisation': auth.loginToken.value,
    };

    return config;
});

export const apiClient = {

    user: {

        async getSelf(): Promise<IUser | Error> {
            try {
                const response = await api.get<IApiResultResponse<IGetSelfResponse>>('/users');

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

        async get(): Promise<IGetCollectionsDto | Error> {
            try {
                const response = await api.get<IApiResultResponse<IGetCollectionsResponse>>('/collections');

                const result = response.data.result;

                return {
                    feedSourceCount: result.feedSourceCount,
                    collections: result.collections.map(x => ({
                        collection: x.collection === null ? null : collectionMapper.map(x.collection),
                        feedSources: x.feedSources.map(feedSourceMapper.map),
                    })),
                };
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

        async add(request: ICreateCollectionRequest): Promise<ICollection | Error> {
            try {
                const response = await api.post<IApiResultResponse<ICreateCollectionResponse>>('/collections', request);

                const collection = response.data.result.collection;

                return {
                    reference: collection.reference,
                    createdAt: dayjs(collection.createdAt),
                    name: collection.name,
                };
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

        async update(collectionReference: string, request: IUpdateCollectionRequest): Promise<ICollection | Error> {
            try {
                const response = await api.put<IApiResultResponse<IUpdateCollectionResponse>>(`/collections/${collectionReference}`, request);

                const collection = response.data.result.collection;

                return {
                    reference: collection.reference,
                    createdAt: dayjs(collection.createdAt),
                    name: collection.name,
                };
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

    },

    feed: {

        async get(): Promise<IGetFeedResponse | Error> {
            try {
                const response = await api.get<IApiResultResponse<IGetFeedResponse>>('/feed');

                return response.data.result;
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

        async addSource(request: IAddFeedSourceRequest): Promise<IAddFeedSourceResponse | Error> {
            try {
                const response = await api.post<IApiResultResponse<IAddFeedSourceResponse>>('/feed', request);

                return response.data.result;
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

        async updateSource(reference: string, request: IUpdateFeedSourceRequest): Promise<IUpdateFeedSourceResponse | Error> {
            try {
                const response = await api.put<IApiResultResponse<IAddFeedSourceResponse>>(`/feed/source/${reference}`, request);

                return response.data.result;
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

        async deleteSource(reference: string): Promise<void | Error> {
            try {
                await api.delete<IApiResultResponse<void>>(`/feed/source/${reference}`);
            }
            catch (error) {
                return responseHelper.handleError(error);
            }
        },

    },

};