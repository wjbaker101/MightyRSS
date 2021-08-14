<template>
    <div class="manage-feed-component flex flex-vertical">
        <div>
            <a :href="feed.websiteUrl" rel="noopener noreferrer" target="_blank">
                {{ feed.title }}
            </a>
            <br>
            <small>{{ feed.rssUrl }}</small>
        </div>
        <div class="delete-feed flex-auto" @click="onDelete">
            &times;
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';

import { FeedSource } from '@/types/FeedSource.type';

export default defineComponent({
    name: 'ManageFeedComponent',

    props: {
        feed: {
            type: Object as PropType<FeedSource>,
            required: true,
        },
    },

    setup() {
        return {}
    },
});
</script>

<style lang="scss">
.manage-feed-component {
    margin: 0.25rem 0;

    &:first-child {
        margin-top: 0;
    }

    &:last-child {
        margin-bottom: 0;
    }

    &:hover {
        .delete-feed {
            opacity: 1;
        }
    }

    .delete-feed {
        padding: 0 0.5rem;
        cursor: pointer;
        border-radius: 0.25rem;
        color: #888;
        opacity: 0;
        transition: all 0.2s;

        &:hover {
            background-color: rgba(0, 0, 0, 0.2);
            color: #222;
        }
    }
}
</style>
