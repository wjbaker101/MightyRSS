import axios, { AxiosInstance } from 'axios';

import { responseHelper } from '@/api/ResponseHelper';

import { LogInRequest, LogInResponse } from '@/api/auth/types/LogIn.type';

class AuthApi {

    private api: AxiosInstance;

    constructor() {
        this.api = axios.create({
            baseURL: '/api/auth',
        });
    }

    async logIn(request: LogInRequest): Promise<LogInResponse | Error> {
        try {
            const response = await this.api.post<LogInResponse>('/login', request);

            return response.data;
        }
        catch (error) {
            return responseHelper.handleError(error);
        }
    }
}

export const authApi = new AuthApi();
