import axios from 'axios';

import { IApiResultResponse, responseHelper } from '@/api/ResponseHelper';

import { ILogInRequest, ILogInResponse } from '@/api/types/LogIn.type';

const api = axios.create({
    baseURL: '/api',
});

export const authClient = {

    async logIn(request: ILogInRequest): Promise<ILogInResponse | Error> {
        try {
            const response = await api.post<IApiResultResponse<ILogInResponse>>('/auth/login', request);

            return response.data.result;
        }
        catch (error) {
            return responseHelper.handleError(error);
        }
    },

};