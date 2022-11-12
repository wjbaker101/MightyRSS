import { ref } from 'vue';

const loginToken = ref<string | null>(null);

export function UseLoginToken() {
    return {
        loginToken,

        clear() {
            loginToken.value = null;
        },
    }
}