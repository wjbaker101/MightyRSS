import { authApi } from '@/api/auth/Auth.api';

import { CacheKey, cacheService } from '@/service/Cache.service';
import { UseLoginToken } from '@/use/LoginToken.use';

import { LogInRequest } from '@/api/auth/types/LogIn.type';

class AuthService {

    async logIn(request: LogInRequest): Promise<string | Error> {
        const logInResponse = await authApi.logIn(request);

        if (logInResponse instanceof Error)
            return logInResponse;

        return logInResponse.jwtToken;
    }

    async loadCache() {
        const cachedLoginToken = await cacheService.get<string>(CacheKey.LOGIN_TOKEN);
        if (cachedLoginToken !== null)
            UseLoginToken().loginToken.value = cachedLoginToken;
    }
}

export const authService = new AuthService();
