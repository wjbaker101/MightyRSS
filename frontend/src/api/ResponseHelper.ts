import axios from 'axios';

import { useAppData } from '@/use/app-data.use';

const appData = useAppData();

const UNAUTHORISED = 401;

export interface ApiResultResponse<T> {
    result: T;
    responseAt: string;
}

class ResponseHelper {

    handleError(error: any): Error {
        if (axios.isAxiosError(error)) {
            if (error.response?.status === UNAUTHORISED)
                appData.auth.logOut();

            return new Error(error.message);
        }

        return new Error('Something went wrong.');
    }
}

export const responseHelper = new ResponseHelper();