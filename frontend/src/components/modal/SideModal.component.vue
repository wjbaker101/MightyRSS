<template>
    <component
        v-if="content !== null"
        :is="content"
        v-bind="contentProps"
        :isOpen="isOpen"
        @close="onClose"
    />
</template>

<script setup lang="ts">
import { DefineComponent, onMounted, onUnmounted, ref, shallowRef } from 'vue';

import { Event, eventService } from '@/service/Event.service';

const isOpen = ref<boolean>(false);
const content = shallowRef<DefineComponent | null>(null);
const contentProps = ref<any>();

const onOpenModal = function (details: any): void {
    contentProps.value = details.props;
    content.value = details.content;
    isOpen.value = true;
};

const onCloseModal = function (): void {
    isOpen.value = false;
};

const onClose = function (): void {
    eventService.publish(Event.CLOSE_MODAL);
};

onMounted(() => {
    eventService.subscribe(Event.OPEN_MODAL, onOpenModal);
    eventService.subscribe(Event.CLOSE_MODAL, onCloseModal);
});

onUnmounted(() => {
    eventService.unsubscribe(Event.OPEN_MODAL, onOpenModal);
    eventService.unsubscribe(Event.CLOSE_MODAL, onCloseModal);
});
</script>

<style lang="scss">
</style>