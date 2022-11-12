<template>
    <AsideComponent />
    <div class="flex flex-columns">
        <div class="aside-placeholder flex-auto"></div>
        <div class="content-width">
            <HeaderComponent />
            <LoginComponent v-if="loginToken === null" />
            <ArticlesFeedComponent v-else />
        </div>
        <div class="aside-placeholder flex-auto"></div>
    </div>
    <SideModalComponent />
    <OpenManageFeedsComponent />
</template>

<script setup lang="ts">
import { onMounted } from 'vue';

import AsideComponent from '@/components/aside/Aside.component.vue';
import HeaderComponent from '@/components/Header.component.vue';
import ArticlesFeedComponent from '@/components/articles/ArticlesFeed.component.vue';
import SideModalComponent from '@/components/modal/SideModal.component.vue';
import LoginComponent from '@/components/login/Login.component.vue';
import OpenManageFeedsComponent from '@/components/OpenManageFeeds.component.vue';

import { UseRss } from '@/use/Rss.use';
import { useAppData } from '@/use/app-data.use';
import { useEvents } from '@/use/events.use';

const appData = useAppData();
const events = useEvents();

const useRss = UseRss();

const loginToken = appData.auth.loginToken;

events.subscribe('ON_LOG_IN', async () => {
    await useRss.load();
});

onMounted(async () => {
    if (loginToken.value !== null)
        await useRss.load();
});
</script>

<style lang="scss">
@import './style/main.scss';

.aside-placeholder {
    width: 350px;

    @media screen and (max-width: BREAKPOINT('mid')) {
        display: none;
    }
}
</style>