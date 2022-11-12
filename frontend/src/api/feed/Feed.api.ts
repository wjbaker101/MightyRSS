import axios, { AxiosInstance, AxiosRequestConfig } from 'axios';

import { ApiResultResponse, responseHelper } from '@/api/ResponseHelper';
import { useAppData } from '@/use/app-data.use';

import { AddFeedSourceRequest, AddFeedSourceResponse } from '@/api/feed/types/AddFeedSource.type';
import { GetFeedResponse } from '@/api/feed/types/GetFeed.type';
import { UpdateFeedSourceRequest } from '@/api/feed/types/UpdateFeedSource.type';

class FeedApi {

    private api: AxiosInstance;

    constructor() {
        const appData = useAppData();

        this.api = axios.create({
            baseURL: '/api/feed',
        });

        this.api.interceptors.request.use((config: AxiosRequestConfig) => {
            config.headers = {
                'Authorisation': appData.auth.loginToken.value,
            };

            return config;
        });
    }

    async getFeed(): Promise<GetFeedResponse | Error> {
        try {
            const response = await this.api.get<ApiResultResponse<GetFeedResponse>>('');

            return response.data.result;
        }
        catch (error) {
            return responseHelper.handleError(error);
        }
    }

    async addFeedSource(request: AddFeedSourceRequest): Promise<AddFeedSourceResponse | Error> {
        try {
            const response = await this.api.post<ApiResultResponse<AddFeedSourceResponse>>('', request);

            return response.data.result;
        }
        catch (error) {
            return responseHelper.handleError(error);
        }
    }

    async updateFeedSource(reference: string, request: UpdateFeedSourceRequest): Promise<AddFeedSourceResponse | Error> {
        try {
            const response = await this.api.put<ApiResultResponse<AddFeedSourceResponse>>(`/source/${reference}`, request);

            return response.data.result;
        }
        catch (error) {
            return responseHelper.handleError(error);
        }
    }

    async deleteFeedSource(reference: string): Promise<void | Error> {
        try {
            await this.api.delete<ApiResultResponse<AddFeedSourceResponse>>(`/source/${reference}`);
        }
        catch (error) {
            return responseHelper.handleError(error);
        }
    }
}

export const feedApi = new FeedApi();