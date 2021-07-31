import { authApi } from '@/api/auth/Auth.api';

import { LogInRequest } from '@/api/auth/types/LogIn.type';

class AuthService {

    async logIn(request: LogInRequest): Promise<string | Error> {
        const logInResponse = await authApi.logIn(request);

        if (logInResponse instanceof Error)
            return logInResponse;

        return logInResponse.jwtToken;
    }
}

export const authService = new AuthService();
