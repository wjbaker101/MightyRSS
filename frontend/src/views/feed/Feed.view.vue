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

import { useAppData } from '@/use/app-data.use';
import { useRss } from '@/use/rss.use';

const appData = useAppData();
const rss = useRss();

const loginToken = appData.auth.loginToken;

onMounted(async () => {
    if (loginToken.value !== null)
        rss.refresh();
});
</script>

<style lang="scss">
.feed-view {

    .aside-placeholder {
        width: 350px;

        @media screen and (max-width: BREAKPOINT('mid')) {
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