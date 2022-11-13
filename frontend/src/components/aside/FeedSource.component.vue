<template>
    <div class="feed-source-component flex flex-vertical">
        <div>
            <a :href="source.websiteUrl" rel="noopener noreferrer" target="_blank">
                {{ (source.titleAlias ?? source.title) }}
            </a>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useRss } from '@/use/rss.use';

import { FeedSource } from '@/model/FeedSource.type';

const props = defineProps<{
    source: FeedSource;
}>();

const rss = useRss();

const feed = rss.feed;

const onDelete = async function (): Promise<void> {
    await rss.deleteSource(props.source.reference);
};
</script>

<style lang="scss">
.feed-source-component {
    padding: 0.25rem 0;

    &:first-child {
        padding-top: 0;
    }

    &:last-child {
        padding-bottom: 0;
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
