import axios from 'axios';

import { useEvents } from '@/use/events.use';

const events = useEvents();

const UNAUTHORISED = 401;

export interface ApiResultResponse<T> {
    result: T;
    responseAt: string;
}

class ResponseHelper {

    handleError(error: any): Error {
        if (axios.isAxiosError(error)) {
            if (error.response?.status === UNAUTHORISED)
                events.publish('ON_LOG_OUT', {});

            return new Error(error.message);
        }

        return new Error('Something went wrong.');
    }
}

export const responseHelper = new ResponseHelper();