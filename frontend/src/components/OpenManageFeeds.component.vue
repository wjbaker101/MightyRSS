<template>
    <div class="open-manage-feeds-component">
        <div @click="onClick">
            <CogIcon />
        </div>
    </div>
</template>

<script setup lang="ts">
import CogIcon from '@/components/icons/CogIcon.vue';
import ManageFeedsModalComponent from '@/components/modal/content/manage-feeds/ManageFeeds.modal.component.vue';

import { Event, eventService } from '@/service/Event.service';

const onClick = function (): void {
    eventService.publish(Event.OPEN_MODAL, {
        content: ManageFeedsModalComponent,
    });
};
</script>

<style lang="scss">
.open-manage-feeds-component {
    padding: 0.5rem;
    position: fixed;
    bottom: 0;
    left: 0;
    line-height: 1em;

    div {
        display: inline-block;
        padding: 0.5rem;
        border-radius: 0.25rem;
        cursor: pointer;

        &:hover {
            background-color: #ccc;
        }
    }
}
</style>