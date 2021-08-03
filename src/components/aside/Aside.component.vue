<template>
    <aside class="aside-component flex flex-vertical">
        <div>
            <h2>Feeds</h2>
            <div>
                <FeedSourceComponent
                    :key="source.reference"
                    v-for="source in feedSources"
                    :source="source"
                />
            </div>
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

        const articles = useRss.articles;

        const feedSources = computed<Array<FeedSource> | null>(() => {
            if (articles.value === null)
                return null;

            const sources = [ ...new Set(articles.value.map(x => x.source)) ];

            return sources
                .sort((a, b) => a.title.localeCompare(b.title));
        });

        return {
            feedSources,
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
