<template>
    <aside class="aside-component flex flex-vertical">
        <div>
            <h2>Feeds</h2>
            <section>
                <FeedSourceComponent
                    :key="source.reference"
                    v-for="source in feedSources"
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
import { computed, defineComponent, ref } from 'vue';

import FeedSourceComponent from '@/components/aside/FeedSource.component.vue';
import UserMessageComponent from '@/components/UserMessage.component.vue';

import { UseRss } from '@/use/Rss.use';
import { UseUserMessage } from '@/use/UserMessage.use';

import { FeedSource } from '@/types/FeedSource.type';
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

        const articles = useRss.articles;

        const newFeed = ref<string>('');
        const newFeedUserMessage = ref<string>('');

        const feedSources = computed<Array<FeedSource> | null>(() => {
            if (articles.value === null)
                return null;

            const sources = [ ...new Set(articles.value.map(x => x.source)) ];

            return sources
                .sort((a, b) => a.title.localeCompare(b.title));
        });

        return {
            feedSources,
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
        margin-top: 0;
    }
}
</style>
