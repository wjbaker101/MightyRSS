import axios from 'axios';

import { useEvents } from '@/use/events/events.use';

const events = useEvents();

const UNAUTHORISED = 401;

export interface IApiResultResponse<T> {
    result: T;
    responseAt: string;
}

export const responseHelper = {

    handleError(error: any): Error {
        if (axios.isAxiosError(error)) {
            if (error.response?.status === UNAUTHORISED)
                events.publish('TRIGGER_LOG_OUT', {});

            return new Error(error.message);
        }

        return new Error('Something went wrong.');
    },

};