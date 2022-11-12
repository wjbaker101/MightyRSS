<template>
    <SideModalContentComponent @close="onClose">
        <h2>Manage Feeds</h2>
        <section>
            <label>
                <strong>New Feed</strong>
                <div class="flex gap">
                    <input type="text" placeholder="https://example.com/rss" v-model="newFeed">
                    <button class="flex-auto" @click="onAddFeed">Add</button>
                </div>
            </label>
            <UserMessageComponent :message="newFeedUserMessage" />
        </section>
        <section>
            <div :key="`collection-${collection}`" v-for="(feeds, collection) in feedsByCollection">
                <h3>{{ collection === 'mighty-rss-no-collection' ? 'Not in a Collection' : collection }}</h3>
                <div>
                    <ManageFeedComponent :key="feed.reference" v-for="feed in feeds" :feed="feed" />
                </div>
            </div>
        </section>
        <section>
            <button @click="onLogOut">Log Out</button>
        </section>
    </SideModalContentComponent>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

import SideModalContentComponent from '@/components/modal/SideModalContent.component.vue';
import UserMessageComponent from '@/components/UserMessage.component.vue';
import ManageFeedComponent from '@/components/modal/content/manage-feeds/ManageFeed.component.vue';

import { feedService } from '@/service/Feed.service';
import { UseRss } from '@/use/Rss.use';
import { UseLoginToken } from '@/use/LoginToken.use';
import { UseUserMessage } from '@/use/UserMessage.use';

import { IFeedSource } from '@/model/FeedSource.type';

const emit = defineEmits(['close']);

const useRss = UseRss();
const useLoginToken = UseLoginToken();
const useUserMessage = UseUserMessage();

const feeds = useRss.feeds;

const newFeed = ref<string>('');
const newFeedUserMessage = ref<string>('');

const feedsByCollection = computed<Record<string, Array<IFeedSource>> | null>(() => {
    if (feeds.value === null)
        return null;

    const value: Record<string, Array<IFeedSource>> = {};

    const feedsForDisplay = feeds.value
        .sort((a, b) => a.title.localeCompare(b.title));

    for (const feed of feedsForDisplay) {
        const collection = feed.collection ?? 'mighty-rss-no-collection';

        if (collection in value) {
            value[collection].push(feed);
            continue;
        }

        value[collection] = [ feed ];
    }

    return value;
});

const sharedAction = function (): void {
    emit('close');
};

const onClose = function (): void {
    sharedAction();
};

const onAddFeed = async function (): Promise<void> {
    if (newFeed.value.length < 5) {
        useUserMessage.set(newFeedUserMessage, 'Please enter a valid feed URL and try again.');
        return;
    }

    const addFeedResponse = await feedService.addFeedSource({
        url: newFeed.value,
    });

    if (addFeedResponse instanceof Error) {
        useUserMessage.set(newFeedUserMessage, addFeedResponse.message);
        return;
    }

    useRss.articles.value = useRss.articles.value?.concat(addFeedResponse) ?? null;

    newFeed.value = '';
};

const onLogOut = function (): void {
    useLoginToken.clear();
    emit('close');
};
</script>

<style lang="scss">
</style>