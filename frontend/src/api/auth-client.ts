import axios from 'axios';

import { ApiResultResponse, responseHelper } from './ResponseHelper';

import { ILogInRequest, ILogInResponse } from './types/LogIn.type';

const api = axios.create({
    baseURL: '/api',
});

export const authClient = {

    async logIn(request: ILogInRequest): Promise<ILogInResponse | Error> {
        try {
            const response = await api.post<ApiResultResponse<ILogInResponse>>('/auth/login', request);

            return response.data.result;
        }
        catch (error) {
            return responseHelper.handleError(error);
        }
    },

};