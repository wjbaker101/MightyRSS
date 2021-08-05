<template>
    <div class="side-modal-content-component" :class="{ 'is-open': isOpen }">
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

<script lang="ts">
import { defineComponent, watch } from 'vue';

import ModalBackdropComponent from '@/components/modal/ModalBackdrop.component.vue';

export default defineComponent({
    name: 'SideModalContentComponent',

    components: {
        ModalBackdropComponent,
    },

    emits: [
        'open',
        'close',
    ],

    props: {
        isOpen: {
            type: Boolean,
            required: true,
        },
    },

    setup(props, { emit }) {
        watch(() => props.isOpen, () => {
            if (!props.isOpen)
                return;

            emit('open');
        });
    },
});
</script>

<style lang="scss">
.side-modal-content-component {
    padding: 1rem;
    margin-left: 1rem;
    position: fixed;
    top: 50%;
    right: 0;
    left: 0;
    opacity: 0;
    pointer-events: none;
    background-color: #fff;
    border-top-left-radius: 0.25rem;
    border-bottom-left-radius: 0.25rem;
    transform: translate(100%, -50%);
    box-shadow: 1px 3px 7px rgba(0, 0, 0, 0.5);
    animation: open 0.5s;
    z-index: 12;
    transition: all 0.5s;

    &.is-open {
        opacity: 1;
        pointer-events: all;
        transform: translate(0, -50%);
    }

    & > h2 {
        margin-top: 0;
    }

    .cross-button {
        margin: -1rem;
        padding: 1rem;
        color: #ccc;
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
            transform: translate(100%, -50%);
        }
        100% {
            opacity: 1;
            transform: translate(0, -50%);
        }
    }
}
</style>
