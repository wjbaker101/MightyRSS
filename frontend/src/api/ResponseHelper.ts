import axios from 'axios';

import { useEvents } from '@/use/events/events.use';

const events = useEvents();

const UNAUTHORISED = 401;

export interface IApiResultResponse<T> {
    result: T;
    responseAt: string;
}

export interface IApiErrorResponse {
    errorMessage: string;
    responseAt: string;
}

export const responseHelper = {

    handleError(error: any): Error {
        if (axios.isAxiosError<IApiErrorResponse>(error)) {
            if (error.response?.status === UNAUTHORISED) {
                events.publish('TRIGGER_LOG_OUT', {});
                return new Error('You are unauthorised, redirecting to the login page.');
            }

            const response = error.response?.data;
            if (!response)
                return new Error(error.message);

            return new Error(response.errorMessage);
        }

        return new Error('Something went wrong.');
    },

};