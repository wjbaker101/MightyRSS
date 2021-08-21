<template>
    <AsideComponent />
    <div class="content-width">
        <h1 class="text-centered">
            <span class="mighty-rss-title" @click="loadFeed">
                <img class="swords-branding" width="48" height="48" src="@/assets/swords.svg">
                <span class="branding-text">Mighty RSS</span>
            </span>
        </h1>
        <LoginComponent v-if="loginToken === null" @login="onLogin" />
        <ArticlesFeedComponent v-else />
    </div>
    <SideModalComponent />
    <OpenManageFeedsComponent />
</template>

<script lang="ts">
import { defineComponent, onMounted } from 'vue';

import AsideComponent from '@/components/aside/Aside.component.vue';
import ArticlesFeedComponent from '@/components/articles/ArticlesFeed.component.vue';
import SideModalComponent from '@/components/modal/SideModal.component.vue';
import LoginComponent from '@/components/login/Login.component.vue';
import OpenManageFeedsComponent from '@/components/OpenManageFeeds.component.vue';

import { authService } from '@/service/Auth.service';
import { UseRss } from '@/use/Rss.use';
import { UseLoginToken } from '@/use/LoginToken.use';

export default defineComponent({
    name: 'App',

    components: {
        AsideComponent,
        ArticlesFeedComponent,
        SideModalComponent,
        LoginComponent,
        OpenManageFeedsComponent,
    },

    setup() {
        const useRss = UseRss();
        const useLoginToken = UseLoginToken();

        const loadFeed = useRss.load;
        const loginToken = useLoginToken.loginToken;

        onMounted(async () => {
            await authService.loadCache();
            await loadFeed();
        });

        return {
            loadFeed,
            loginToken,

            async onLogin() {
                await loadFeed();
            },
        }
    },
});
</script>

<style lang="scss">
@import './style/main.scss';

.mighty-rss-title {
    cursor: pointer;
    user-select: none;
}

.swords-branding,
.branding-text {
    vertical-align: middle;
}

.swords-branding {
    margin-right: 1rem;
}
</style>
