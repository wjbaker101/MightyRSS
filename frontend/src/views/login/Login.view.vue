<template>
    <div class="login-view">
        <div class="content-width">
            <h2>Log In</h2>
            <section>
                <label>
                    <strong>Username</strong>
                    <input
                        ref="usernameInput"
                        type="text"
                        v-model="username"
                        @keyup.enter="onUsernameEnter"
                    >
                </label>
                <label>
                    <strong>Password</strong>
                    <input
                        ref="passwordInput"
                        type="password"
                        v-model="password"
                        @keyup.enter="onPasswordEnter"
                    >
                </label>
            </section>
            <section>
                <button @click="onLogIn">Log In</button>
            </section>
            <section>
                <UserMessageComponent />
            </section>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';

import UserMessageComponent, { useUserMessage } from '@/components/UserMessage.component.vue';

import { useAppData } from '@/use/app-data.use';

const appData = useAppData();
const router = useRouter();
const userMessage = useUserMessage();

const usernameInput = ref<HTMLInputElement | null>(null);
const passwordInput = ref<HTMLInputElement | null>(null);

const username = ref<string>('');
const password = ref<string>('');

const logIn = async function (): Promise<void> {
    if (username.value.length < 3) {
        userMessage.show('Username is not valid, please try again.');
        return;
    }
    if (password.value.length < 3) {
        userMessage.show('Password is not valid, please try again.');
        return;
    }

    await appData.auth.logIn(username.value, password.value);
    router.push({ path: '/' });
};

const onLogIn = async function (): Promise<void> {
    await logIn();
};

const onUsernameEnter = async function (): Promise<void> {
    if (password.value.length === 0) {
        passwordInput.value?.focus();
        return;
    }

    await logIn();
};

const onPasswordEnter = async function (): Promise<void> {
    if (username.value.length === 0) {
        usernameInput.value?.focus();
        return;
    }

    await logIn();
};
</script>

<style lang="scss">
</style>