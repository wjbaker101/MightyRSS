<template>
    <aside class="aside-component flex flex-vertical">
        <div>
            <section class="flex flex-bottom">
                <h2 class="flex-auto">Feeds</h2>
                <div class="feeds-count" v-if="feeds !== null">
                    <small>({{ feeds.length }})</small>
                </div>
            </section>
            <section>
                <FeedSourceComponent
                    :key="source.reference"
                    v-for="source in feeds"
                    :source="source"
                />
            </section>
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
        </div>
    </aside>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';

import FeedSourceComponent from '@/components/aside/FeedSource.component.vue';
import UserMessageComponent from '@/components/UserMessage.component.vue';

import { UseRss } from '@/use/Rss.use';
import { UseUserMessage } from '@/use/UserMessage.use';

import { feedService } from '@/service/Feed.service';

export default defineComponent({
    name: 'AsideComponent',

    components: {
        FeedSourceComponent,
        UserMessageComponent,
    },

    setup() {
        const useRss = UseRss();
        const useUserMessage = UseUserMessage();

        const feeds = useRss.feeds;

        const newFeed = ref<string>('');
        const newFeedUserMessage = ref<string>('');

        return {
            feeds,
            newFeed,
            newFeedUserMessage,

            async onAddFeed() {
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
            },
        }
    },
});
</script>

<style lang="scss">
.aside-component {
    width: 350px;
    max-width: 100%;
    padding: 1rem 0.5rem;
    position: fixed;
    top: 0;
    left: 0;
    bottom: 0;

    h2 {
        margin: 0;
    }

    .feeds-count {
        margin-left: 0.5rem;
    }
}
</style>
