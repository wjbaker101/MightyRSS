import axios from 'axios';

class ResponseHelper {

    handleError(error: any): Error {
        if (axios.isAxiosError(error))
            return new Error(error.message);

        return new Error('Something went wrong.');
    }
}

export const responseHelper = new ResponseHelper();
