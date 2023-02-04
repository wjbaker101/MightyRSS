import { readonly, ref } from 'vue';

import { authClient } from '@/api/auth-client';

import { CacheKey, useCache } from '@/use/cache.use';

const cache = useCache();

const loginToken = ref<string | null>(cache.get(CacheKey.LOGIN_TOKEN));

export const useAppData = function () {
    return {

        auth: {
            loginToken: readonly(loginToken),

            async logIn(username: string, password: string): Promise<void> {
                const result = await authClient.logIn({
                    username,
                    password,
                });
                if (result instanceof Error)
                    return;

                loginToken.value = result.jwtToken;

                cache.set(CacheKey.LOGIN_TOKEN, loginToken.value);
            },

            logOut(): void {
                loginToken.value = null;
                cache.clear(CacheKey.LOGIN_TOKEN);
            },
        },

    };
};