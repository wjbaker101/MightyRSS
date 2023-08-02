<template>
    <div class="feed-view">
        <AsideComponent />
        <div class="flex flex-columns">
            <div class="aside-placeholder flex-auto"></div>
            <div class="content-width">
                <ArticlesFeedComponent />
            </div>
            <div class="aside-placeholder flex-auto"></div>
        </div>
        <div class="open-configuration-container">
            <RouterLink to="/configuration">
                <IconComponent icon="settings" />
            </RouterLink>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';

import AsideComponent from '@/components/aside/Aside.component.vue';
import ArticlesFeedComponent from '@/views/feed/components/ArticlesFeed.component.vue';

import { useRss } from '@/use/rss.use';

const rss = useRss();

onMounted(async () => {
    rss.refresh();
});
</script>

<style lang="scss">
.feed-view {

    .aside-placeholder {
        width: 350px;

        @media screen and (max-width: var(--breakpoint-mid)) {
            display: none;
        }
    }

    .open-configuration-container {
        padding: 0.5rem;
        position: fixed;
        bottom: 0;
        left: 0;
        line-height: 1em;
    }
}
</style>