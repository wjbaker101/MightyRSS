<template>
    <div class="user-message-component" v-if="message !== ''">
        {{ message }}
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    name: 'UserMessageComponent',

    props: {
        message: {
            type: String,
            required: true,
        },
    },
});
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
