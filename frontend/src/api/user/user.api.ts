import axios, { AxiosRequestConfig } from 'axios';
import dayjs from 'dayjs';

import { useAppData } from '@/use/app-data.use';

import { IUser } from '@/model/User.model';
import { IGetSelfResponse } from './types/GetSelf.type';
import { ApiResultResponse } from '../ResponseHelper';

const appData = useAppData();

const api = axios.create({
    baseURL: '/api/users',
});

api.interceptors.request.use((config: AxiosRequestConfig) => {
    config.headers = {
        'Authorisation': appData.auth.loginToken.value,
    };

    return config;
});

export const userApi = {

    async getSelf(): Promise<IUser> {
        const response = await api.get<ApiResultResponse<IGetSelfResponse>>('');

        const user = response.data.result.user;

        return {
            reference: user.reference,
            createdAt: dayjs(user.createdAt),
            username: user.username,
        };
    },

};