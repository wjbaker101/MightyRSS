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
                    :key="feed.reference"
                    v-for="feed in feeds"
                    :source="feed"
                />
            </section>
        </div>
    </aside>
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