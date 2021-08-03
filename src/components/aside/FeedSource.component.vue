<template>
    <div class="feed-source-component flex">
        <div>
            <a :href="source.websiteUrl" rel="noopener noreferrer" target="_blank">
                {{ source.title }}
            </a>
        </div>
        <div class="delete-feed flex-auto" @click="onDelete">
            &times;
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';

import { feedService } from '@/service/Feed.service';
import { UseRss } from '@/use/Rss.use';

import { FeedSource } from '@/types/FeedSource.type';

export default defineComponent({
    name: 'FeedSourceComponent',

    props: {
        source: {
            type: Object as PropType<FeedSource>,
            required: true,
        },
    },

    setup(props) {
        const useRss = UseRss();

        return {
            async onDelete() {
                await feedService.deleteFeedSource(props.source.reference);

                if (useRss.articles.value === null)
                    return;

                useRss.articles.value = useRss.articles.value
                    .filter(x => x.source.reference !== props.source.reference);
            },
        }
    },
});
</script>

<style lang="scss">
.feed-source-component {
    padding: 0.25rem 0.5rem;

    .delete-feed {
        margin: -0.25rem -0.5rem;
        padding: 0.25rem 0.5rem;
        cursor: pointer;
        border-radius: 0.25rem;
        color: #888;
        transition: all 0.2s;

        &:hover {
            background-color: rgba(0, 0, 0, 0.2);
            color: #222;
        }
    }
}
</style>
