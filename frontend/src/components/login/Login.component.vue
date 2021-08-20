<template>
    <div class="login-component">
        <h2>Log In</h2>
        <section>
            <label>
                <strong>Username</strong>
                <input type="text" placeholder="" v-model="username">
            </label>
            <label>
                <strong>Password</strong>
                <input type="password" placeholder="" v-model="password">
            </label>
        </section>
        <section>
            <button @click="onLogIn">Log In</button>
            <UserMessageComponent :message="userMessage" />
        </section>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';

import UserMessageComponent from '@/components/UserMessage.component.vue';

import { authService } from '@/service/Auth.service';
import { cacheService, CacheKey } from '@/service/Cache.service';
import { UseLoginToken } from '@/use/LoginToken.use';
import { UseUserMessage } from '@/use/UserMessage.use';

export default defineComponent({
    name: 'LoginComponent',

    components: {
        UserMessageComponent,
    },

    emits: [
        'login',
    ],

    setup(_, { emit }) {
        const useLoginToken = UseLoginToken();
        const useUserMessage = UseUserMessage();

        const loginToken = useLoginToken.loginToken;

        const username = ref<string>('');
        const password = ref<string>('');
        const userMessage = ref<string>('');

        return {
            username,
            password,
            userMessage,

            async onLogIn() {
                const logInResponse = await authService.logIn({
                    username: username.value,
                    password: password.value,
                });
                if (logInResponse instanceof Error) {
                    useUserMessage.set(userMessage, logInResponse.message);
                    return;
                }

                loginToken.value = logInResponse;

                await cacheService.set(CacheKey.LOGIN_TOKEN, loginToken.value);

                emit('login');
            },
        }
    },
});
</script>

<style lang="scss">
</style>
