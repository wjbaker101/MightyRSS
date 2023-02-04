<template>
    <div class="login-component">
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
            <UserMessageComponent :message="userMessage" />
        </section>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import UserMessageComponent from '@/components/UserMessage.component.vue';

import { UseUserMessage } from '@/use/UserMessage.use';
import { useAppData } from '@/use/app-data.use';
import { useEvents } from '@/use/events/events.use';

const appData = useAppData();
const events = useEvents();

const useUserMessage = UseUserMessage();

const usernameInput = ref<HTMLInputElement | null>(null);
const passwordInput = ref<HTMLInputElement | null>(null);

const username = ref<string>('');
const password = ref<string>('');
const userMessage = ref<string>('');

const logIn = async function (): Promise<void> {
    if (username.value.length < 3) {
        useUserMessage.set(userMessage, 'Username is not valid, please try again.');
        return;
    }
    if (password.value.length < 3) {
        useUserMessage.set(userMessage, 'Password is not valid, please try again.');
        return;
    }

    await appData.auth.logIn(username.value, password.value);
    events.publish('ON_LOG_IN', {});
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