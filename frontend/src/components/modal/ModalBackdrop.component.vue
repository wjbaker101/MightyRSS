<template>
    <div
        class="modal-backdrop-component"
        :class="{ 'is-visible': isVisible }"
        :style="{ 'z-index': zIndex }"
        @click="onClose"
    >
    </div>
</template>

<script setup lang="ts">
defineProps<{
    isVisible: boolean;
    zIndex?: {
        type: Number,
        default: 11,
    };
}>();

const emit = defineEmits(['close']);

const onClose = function (): void {
    emit('close');
};
</script>

<style lang="scss">
.modal-backdrop-component {
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    background-color: rgba(10, 10, 40, 0.5);
    opacity: 0;
    pointer-events: none;

    &.is-visible {
        opacity: 1;
        pointer-events: all;
    }
}
</style>