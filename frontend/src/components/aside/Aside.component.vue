<template>
    <aside class="aside-component flex flex-vertical">
        <div>
            <section class="flex flex-bottom">
                <h2 class="flex-auto">Feeds</h2>
                <div class="feeds-count" v-if="feedsForDisplay !== null">
                    <small>({{ feedsForDisplay.length }})</small>
                </div>
            </section>
            <section>
                <FeedSourceComponent
                    :key="feed.reference"
                    v-for="feed in feedsForDisplay"
                    :source="feed"
                />
            </section>
        </div>
    </aside>
</template>

<script lang="ts">
import { computed, defineComponent } from 'vue';

import FeedSourceComponent from '@/components/aside/FeedSource.component.vue';

import { UseRss } from '@/use/Rss.use';

import { FeedSource } from '@/types/FeedSource.type';

export default defineComponent({
    name: 'AsideComponent',

    components: {
        FeedSourceComponent,
    },

    setup() {
        const useRss = UseRss();

        const feeds = useRss.feeds;

        const feedsForDisplay = computed<Array<FeedSource> | null>(() => {
            if (feeds.value === null)
                return null;

            return feeds.value
                .sort((a, b) => a.title.localeCompare(b.title));
        });

        return {
            feedsForDisplay,
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

    @media screen and (max-width: BREAKPOINT('mid')) {
        .flex {
            display: none;
        }
    }

    h2 {
        margin: 0;
    }

    .feeds-count {
        margin-left: 0.5rem;
    }
}
</style>
