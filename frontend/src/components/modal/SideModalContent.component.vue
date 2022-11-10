<template>
    <div class="side-modal-content-component parchment-background" :class="{ 'is-open': isOpen }">
        <div class="flex">
            <div></div>
            <div class="cross-button flex-auto" @click="$emit('close')">
                &times;
            </div>
        </div>
        <slot />
    </div>
    <ModalBackdropComponent :isVisible="isOpen" @close="$emit('close')" />
</template>

<script setup lang="ts">
import { watch } from 'vue';

import ModalBackdropComponent from '@/components/modal/ModalBackdrop.component.vue';

const props = defineProps<{
    isOpen: boolean;
}>();

const emit = defineEmits(['open', 'close']);

watch(() => props.isOpen, () => {
    if (!props.isOpen)
        return;

    emit('open');
});
</script>

<style lang="scss">
.side-modal-content-component {
    width: 720px;
    max-width: 100%;
    padding: 1rem;
    margin: 1rem 0 1rem 1rem;
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    opacity: 0;
    pointer-events: none;
    border-top-left-radius: 0.25rem;
    border-bottom-left-radius: 0.25rem;
    transform: translateX(100%);
    box-shadow: 1px 3px 7px rgba(0, 0, 0, 0.5);
    animation: open 0.5s;
    z-index: 12;
    transition: all 0.5s;

    &.is-open {
        opacity: 1;
        pointer-events: all;
        transform: translateX(0);
    }

    & > h2 {
        margin-top: 0;
    }

    .cross-button {
        margin: -1rem;
        padding: 1rem;
        cursor: pointer;
        border-bottom-left-radius: 0.25rem;
        line-height: 0;
        transition: all 0.2s;

        &:hover {
            color: #222;
            background-color: #ccc;
        }
    }

    @keyframes open {
        0% {
            opacity: 0;
            transform: translate(100%, 0);
        }
        100% {
            opacity: 1;
            transform: translate(0, 0);
        }
    }
}
</style>