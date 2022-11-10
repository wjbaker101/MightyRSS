<template>
    <div class="manage-feed-component flex flex-vertical">
        <div>
            <HiddenTextBoxComponent v-model="displayTitle" @finish="onTitleFinish" />
            <br>
            <small>{{ feed.rssUrl }}</small>
        </div>
        <div class="delete-feed flex-auto" @click="onDelete">
            &times;
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import HiddenTextBoxComponent from '@/components/HiddenTextBox.component.vue';

import { FeedSource } from '@/model/FeedSource.type';
import { feedApi } from '@/api/feed/Feed.api';

const props = defineProps<{
    feed: FeedSource;
}>();

const displayTitle = computed<string>({
    get() {
        return props.feed.titleAlias ?? props.feed.title;
    },
    set(title) {
        props.feed.titleAlias = title;
    },
});

const onTitleFinish = async function (newTitle: string): Promise<void> {
    await feedApi.updateFeedSource(props.feed.reference, {
        collection: props.feed.collection,
        title: newTitle,
    });
};
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
        opacity: 0;
        transition: all 0.2s;

        &:hover {
            background-color: rgba(0, 0, 0, 0.2);
        }
    }
}
</style>