<template>
    <div class="content-width">
        <h1 class="text-centered">
            <img class="swords-branding" width="48" height="48" src="@/assets/swords.svg">
            <span class="branding-text">Mighty RSS</span>
        </h1>
        <main class="articles-container">
            <div v-if="articlesToday !== null">
                <h2>Today</h2>
                <ArticleComponent :key="article.reference" v-for="article in articlesToday" :article="article" />
            </div>
            <div v-if="articlesYesterday !== null">
                <h2>Yesterday</h2>
                <ArticleComponent :key="article.reference" v-for="article in articlesYesterday" :article="article" />
            </div>
            <div v-if="articlesPrevious !== null">
                <h2>Previous</h2>
                <ArticleComponent :key="article.reference" v-for="article in articlesPrevious" :article="article" />
            </div>
        </main>
    </div>
    <FeedSourcesComponent />
</template>

<script lang="ts">
import { computed, defineComponent, onMounted } from 'vue';

import ArticleComponent from '@/components/Article.component.vue';
import FeedSourcesComponent from '@/components/FeedSources.component.vue';

import { authService } from '@/service/Auth.service';
import { feedService } from '@/service/Feed.service';
import { UseLoginToken } from '@/use/LoginToken.use';
import { UseRss } from '@/use/Rss.use';

import { FeedArticle } from '@/types/FeedArticle.type';

export default defineComponent({
    name: 'App',

    components: {
        ArticleComponent,
        FeedSourcesComponent,
    },

    setup() {
        const useLoginToken = UseLoginToken();
        const useRss = UseRss();

        const articles = useRss.articles;

        const articlesToday = computed<Array<FeedArticle> | null>(() => {
            if (articles.value === null)
                return null;

            return articles.value.filter(x => x.publishedAt.isToday());
        });

        const articlesYesterday = computed<Array<FeedArticle> | null>(() => {
            if (articles.value === null)
                return null;

            return articles.value.filter(x => x.publishedAt.isYesterday());
        });

        const articlesPrevious = computed<Array<FeedArticle> | null>(() => {
            if (articles.value === null)
                return null;

            return articles.value.filter(x => !x.publishedAt.isToday() && !x.publishedAt.isYesterday());
        });

        onMounted(async () => {
            const logInResponse = await authService.logIn({
                username: 'TestUsername',
                password: 'TestPassword',
            });

            if (logInResponse instanceof Error)
                return;

            useLoginToken.loginToken.value = logInResponse;

            const feed = await feedService.getFeed();
            if (feed instanceof Error)
                return;

            articles.value = feed
                .sort((a, b) => {
                    if (a.publishedAt.isBefore(b.publishedAt)) return 1;
                    if (a.publishedAt.isAfter(b.publishedAt)) return -1;
                    return 0;
                });
        });

        return {
            articlesToday,
            articlesYesterday,
            articlesPrevious,
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
    background-color: #fff;
    border: 0;
    border-radius: 0.5rem;
    font: inherit;
}

button {
    padding: 0.5rem 1rem;
    background-color: #fff;
    border: 0;
    border-radius: 0.5rem;
    font: inherit;
    cursor: pointer;
}

.text-centered {
    text-align: center;
}

.content-width {
    max-width: 600px;
    margin-left: auto;
    margin-right: auto;
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

    .flex-auto {
        flex: 0 0 auto;
    }
}
</style>
