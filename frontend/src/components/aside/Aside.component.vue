<template>
    <CardComponent class="aside-component flex flex-vertical">
        <div class="flex-auto">
            <section class="flex flex-bottom">
                <h2 class="flex-auto">Feeds</h2>
                <div class="feeds-count" v-if="feeds !== null">
                    <small>({{ feeds.length }})</small>
                </div>
            </section>
            <section>
                <FeedSourceComponent
                    :key="feed.reference"
                    v-for="feed in feeds"
                    :feed-source="feed"
                />
            </section>
        </div>
    </CardComponent>
</template>

<script setup lang="ts">
import FeedSourceComponent from '@/components/aside/FeedSource.component.vue';

import { useRss } from '@/use/rss.use';

const rss = useRss();

const feeds = rss.sources;
</script>

<style lang="scss">
.aside-component {
    width: 350px;
    max-width: 100%;
    padding: 1rem 0.5rem;
    position: fixed;
    top: 50%;
    left: 0.25rem;
    transform: translateY(-50%);

    @media screen and (max-width: var(--breakpoint-mid)) {
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

    & > div {
        margin-top: auto;
        margin-bottom: auto;
    }
}
</style>