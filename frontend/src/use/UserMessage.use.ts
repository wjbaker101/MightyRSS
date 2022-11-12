import { Ref } from 'vue';

const DISPLAY_MESSAGE_TIME = 6000;

export function UseUserMessage() {
    return {
        set(userMessage: Ref<string>, message: string) {
            userMessage.value = message;

            setTimeout(() => {
                userMessage.value = '';
            }, DISPLAY_MESSAGE_TIME);
        },
    }
}