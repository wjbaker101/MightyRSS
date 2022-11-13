<template>
    <div class="manage-feed-component flex flex-vertical">
        <div>
            <HiddenTextBoxComponent v-model="displayTitle" @finish="onTitleFinish" />
            <br>
            <small>{{ feedSource.rssUrl }}</small>
        </div>
        <div class="delete-feed flex-auto" @click="onDelete">
            &times;
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import HiddenTextBoxComponent from '@/components/HiddenTextBox.component.vue';

import { feedApi } from '@/api/feed/Feed.api';
import { useRss } from '@/use/rss.use';

import { IFeedSource } from '@/model/FeedSource.type';

const props = defineProps<{
    feedSource: IFeedSource;
}>();

const rss = useRss();

const displayTitle = computed<string>({
    get() {
        return props.feedSource.titleAlias ?? props.feedSource.title;
    },
    set(title) {
        props.feedSource.titleAlias = title;
    },
});

const onTitleFinish = async function (newTitle: string): Promise<void> {
    await feedApi.updateFeedSource(props.feedSource.reference, {
        collection: props.feedSource.collection,
        title: newTitle,
    });
};

const onDelete = async function (): Promise<void> {
    await rss.deleteSource(props.feedSource.reference);
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