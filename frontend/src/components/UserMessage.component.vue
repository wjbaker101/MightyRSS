<template>
    <div class="user-message-component" v-if="message !== null">
        {{ message }}
    </div>
</template>

<script setup lang="ts">
//
</script>

<script lang="ts">
import { ref } from 'vue';

const message = ref<string | null>(null);

export const useUserMessage = function () {
    return {

        show(value: string): void {
            message.value = value;

            setTimeout(() => {
                message.value = null;
            }, 6000);
        },

    };
};
</script>

<style lang="scss">
.user-message-component {
    margin-top: 0.5rem;
    padding: 0.5rem;
    background-color: rgba(161, 16, 16, 0.2);
    color: #990a0a;
    border: 1px solid #990a0a;
    border-radius: 0.25rem;
}
</style>