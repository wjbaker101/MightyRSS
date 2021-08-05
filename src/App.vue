<template>
    <AsideComponent />
    <div class="content-width">
        <h1 class="text-centered">
            <span class="mighty-rss-title" @click="refreshFeed">
                <img class="swords-branding" width="48" height="48" src="@/assets/swords.svg">
                <span class="branding-text">Mighty RSS</span>
            </span>
        </h1>
        <div v-if="displayArticles === null">
            Loading...
        </div>
        <main v-else class="articles-container">
            <div v-if="articlesToday !== null">
                <h2>Today</h2>
                <ArticleComponent :key="article.reference" v-for="article in articlesToday" :article="article" />
            </div>
            <div v-if="articlesToday.length === 0">
                <p>Nothing for today!</p>
            </div>
            <div v-if="articlesYesterday !== null && articlesYesterday.length > 0">
                <h2>Yesterday</h2>
                <ArticleComponent :key="article.reference" v-for="article in articlesYesterday" :article="article" />
            </div>
            <div v-if="articlesPrevious !== null && articlesPrevious.length > 0">
                <h2>Previous</h2>
                <ArticleComponent :key="article.reference" v-for="article in articlesPrevious" :article="article" />
                <div class="expand-articles-container text-centered" v-if="!isArticlesExpanded">
                    <button @click="expandArticles">Show More</button>
                </div>
            </div>
        </main>
    </div>
    <SideModalComponent />
    <OpenManageFeedsComponent />
</template>

<script lang="ts">
import { computed, defineComponent, onMounted, ref } from 'vue';
import dayjs from 'dayjs';

import AsideComponent from '@/components/aside/Aside.component.vue';
import ArticleComponent from '@/components/Article.component.vue';
import SideModalComponent from '@/components/modal/SideModal.component.vue';
import OpenManageFeedsComponent from '@/components/OpenManageFeeds.component.vue';

import { authService } from '@/service/Auth.service';
import { feedService } from '@/service/Feed.service';
import { UseLoginToken } from '@/use/LoginToken.use';
import { UseRss } from '@/use/Rss.use';

import { FeedArticle } from '@/types/FeedArticle.type';

export default defineComponent({
    name: 'App',

    components: {
        AsideComponent,
        ArticleComponent,
        SideModalComponent,
        OpenManageFeedsComponent,
    },

    setup() {
        const useLoginToken = UseLoginToken();
        const useRss = UseRss();

        const articles = useRss.articles;
        const isArticlesExpanded = ref<boolean>(false);

        const displayArticles = computed<Array<FeedArticle> | null>(() => {
            if (articles.value === null)
                return null;

            const twoMonthsAgo = dayjs().subtract(2, 'months');

            return articles.value
                .filter(x => isArticlesExpanded.value || (!isArticlesExpanded.value && x.publishedAt.isAfter(twoMonthsAgo)))
                .sort((a, b) => {
                    if (a.publishedAt.isBefore(b.publishedAt)) return 1;
                    if (a.publishedAt.isAfter(b.publishedAt)) return -1;
                    return 0;
                });
        });

        const articlesToday = computed<Array<FeedArticle> | null>(() => {
            if (displayArticles.value === null)
                return null;

            return displayArticles.value.filter(x => x.publishedAt.isToday());
        });

        const articlesYesterday = computed<Array<FeedArticle> | null>(() => {
            if (displayArticles.value === null)
                return null;

            return displayArticles.value.filter(x => x.publishedAt.isYesterday());
        });

        const articlesPrevious = computed<Array<FeedArticle> | null>(() => {
            if (displayArticles.value === null)
                return null;

            return displayArticles.value
                .filter(x => !x.publishedAt.isToday() && !x.publishedAt.isYesterday());
        });

        const refreshFeed = async function () {
            const logInResponse = await authService.logIn({
                username: 'TestUsername',
                password: 'TestPassword',
            });

            if (logInResponse instanceof Error)
                return;

            useLoginToken.loginToken.value = logInResponse;

            articles.value = null;

            const feed = await feedService.getFeed();
            if (feed instanceof Error)
                return;

            articles.value = feed;
        };

        onMounted(async () => {
            await refreshFeed();
        });

        return {
            displayArticles,
            articlesToday,
            articlesYesterday,
            articlesPrevious,
            isArticlesExpanded,

            refreshFeed,

            expandArticles() {
                isArticlesExpanded.value = true;
            },
        }
    },
});
</script>

<style lang="scss">
html,
body {
    height: 100%;
}

body {
    margin: 0;
    font-size: 16px;
    font-family: 'Gowun Batang', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    letter-spacing: 0.5px;
    line-height: 1.6em;
    background-color: #eace9a;
    background:
        url(data:image/png;base64,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),
        radial-gradient(circle, #eace9a 35%, #d4b06c 75%);
    color: #222;
}

*,
*::before,
*::after {
    box-sizing: border-box;
}

h1, h2, h3, h4, h5, h6 {
    font-family: 'Dancing Script', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

h1 {
    font-size: 2.5rem;
}

h3 {
    font-family: 'Gowun Batang', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

a {
    color: inherit;
    text-decoration: underline;

    &:hover {
        text-decoration: none;
    }
}

input[type=text] {
    width: 100%;
    padding: 0.5rem 1rem;
    background-color: #eace9a;
    background:
        url(data:image/png;base64,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),
        radial-gradient(circle, #dfb972 35%, #be9342 75%);
    box-shadow: 1px 1px 6px rgba(0, 0, 0, 0.5) inset;
    border: 0;
    border-radius: 0.5rem;
    font: inherit;
    letter-spacing: inherit;
}

button {
    padding: 0.5rem 1rem;
    background-color: #777;
    background:
        url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyBAMAAADsEZWCAAAAD1BMVEXg4ODe3t7Z2dnh4eHMzMwfFvLJAAAABXRSTlMpHxQzCglRi48AAAInSURBVHheTZOBjR0xCEQfpgEgDRiUAji5g+u/qNzoS0lmJe9KDwyDvbytmXj4WlhO+9rdSAqwDIe3sFbXN2NzKE77e0/rspyAfu9MJxXvR+APlA8JpydJfzxp7VzrirTpimpI8lMIC+C/zPlRc8L3H2mAmOFcmwBrHAjss1n1hySbgFY4jSSrpgw+PYZ6NaRMhxI5AOGIyOcic/F15bSxONeQ2IAzfZrjCyqj7bG7CZYzE8ffYzEVhuTrumHTFuaAXQvgrfpfc5tL6pu6xluAC5TbqDYDE+Ai1gB+RDIH68AcIGuuv63vS9QkWeGG1JmAW8dmZoAFhyJJlVM6W7+M91DXv0kkhwTqFwqTQBdAI7KJzYq3D0BLAYL0X+cZRqUI+EJyREBDutUksams0oTD3873PTPBf6qL+WO+sUh4bxsdvNP48bW5uoB6toEaMG0IdvEHPBS7ZwKbexo2AIwdrmMB1sqQwNdgiKX4EHhITpKfAkjCAI5X08laKgYCha6GJcID/BW6DifIbd4SBISQBauc7EzeAxzucpxhMT8zk/rZTIR1HMuwgGnqgpfIW3jyLj91txpjAJ5leQE2SbF2qRaJtU4RqACZILN41syFcr1cJOwCPMMSKEgWsT7XRdiMd6JaR3XGsZnrhgSU07pAM8BMQKGVGt/8ITr/FFlG5K1Ny9dGKd4qXP0FD5EFi6PZkEEFMq1I8Qs1ndUwQIbIBQiwCdRg8gfxVEji4SgRmgAAAABJRU5ErkJggg==)
        #777;
    border: 2px solid #666;
    outline: 1px solid #777;
    color: #fff;
    text-shadow: 1px 1px 1px rgba(0, 0, 0, 0.7);
    border-radius: 0.5rem;
    font: inherit;
    cursor: pointer;
    transition: all 0.2s;

    &:hover {
        border-color: #222;
    }
}

section + section {
    margin-top: 1rem;
}

.text-centered {
    text-align: center;
}

.content-width {
    max-width: 600px;
    margin-left: auto;
    margin-right: auto;
}

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

.articles-container {
    h2 {
        margin: 2.5rem 0;
    }
}

.flex {
    display: flex;

    &.gap {
        gap: 0.5rem;
    }

    &.gap-small {
        gap: 0.25rem;
    }

    & > * {
        flex: 1;
    }

    &.flex-vertical {
        align-items: center;
    }

    &.flex-bottom {
        align-items: baseline;
    }

    .flex-auto {
        flex: 0 0 auto;
    }
}

.expand-articles-container {
    padding: 2rem 0;
}
</style>
