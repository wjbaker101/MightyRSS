import axios from 'axios';

import { UseLoginToken } from '@/use/LoginToken.use';

const useLoginToken = UseLoginToken();

const UNAUTHORISED = 401;

class ResponseHelper {

    handleError(error: any): Error {
        if (axios.isAxiosError(error)) {
            if (error.response?.status === UNAUTHORISED)
                useLoginToken.clear();

            return new Error(error.message);
        }

        return new Error('Something went wrong.');
    }
}

export const responseHelper = new ResponseHelper();
