import axios, { AxiosInstance, AxiosRequestConfig } from 'axios';

import { responseHelper } from '@/api/ResponseHelper';
import { UseLoginToken } from '@/use/LoginToken.use';

import { AddFeedSourceRequest, AddFeedSourceResponse } from '@/api/feed/types/AddFeedSource.type';
import { GetFeedResponse } from '@/api/feed/types/GetFeed.type';

class FeedApi {

    private api: AxiosInstance;

    constructor() {
        const useLoginToken = UseLoginToken();

        this.api = axios.create({
            baseURL: '/api/feed',
        });

        this.api.interceptors.request.use((config: AxiosRequestConfig) => {
            config.headers['Authorisation'] = useLoginToken.loginToken.value;

            return config;
        });
    }

    async getFeed(): Promise<GetFeedResponse | Error> {
        try {
            const response = await this.api.get<GetFeedResponse>('');

            return response.data;
        }
        catch (error) {
            return responseHelper.handleError(error);
        }
    }

    async addFeedSource(request: AddFeedSourceRequest): Promise<AddFeedSourceResponse | Error> {
        try {
            const response = await this.api.post<AddFeedSourceResponse>('', request);

            return response.data;
        }
        catch (error) {
            return responseHelper.handleError(error);
        }
    }
}

export const feedApi = new FeedApi();
