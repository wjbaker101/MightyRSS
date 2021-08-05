<template>
    <SideModalContentComponent @close="onClose">
        <h2>Manage Feeds</h2>
    </SideModalContentComponent>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

import SideModalContentComponent from '@/components/modal/SideModalContent.component.vue';

export default defineComponent({
    name: 'ManageFeedsModalComponent',

    components: {
        SideModalContentComponent,
    },

    setup(_, { emit }) {
        const sharedAction = function () {
            emit('close');
        };

        return {
            onYes() {
                sharedAction();
            },

            onNo() {
                sharedAction();
            },

            onClose() {
                sharedAction();
            },
        }
    },
});
</script>

<style lang="scss">
</style>
