<template>
    <div v-if="articlesForDisplay === null">
        Loading...
    </div>
    <div v-else class="articles-feed-component">
        <div v-if="articlesToday !== null">
            <h2>Today</h2>
            <ArticleComponent
                :key="article.reference"
                v-for="article in articlesToday"
                :article="article"
            />
        </div>
        <div v-if="articlesToday.length === 0">
            <p>Nothing for today!</p>
        </div>
        <div v-if="articlesYesterday !== null && articlesYesterday.length > 0">
            <h2>Yesterday</h2>
            <ArticleComponent
                :key="article.reference"
                v-for="article in articlesYesterday"
                :article="article"
            />
        </div>
        <div v-if="articlesPrevious !== null && articlesPrevious.length > 0">
            <h2>Previous</h2>
            <ArticleComponent
                :key="article.reference"
                v-for="article in articlesPrevious"
                :article="article"
            />
            <div class="expand-container text-centered" v-if="!isArticlesExpanded">
                <button @click="expandArticles">Show More</button>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from 'vue';
import dayjs from 'dayjs';

import ArticleComponent from '@/components/Article.component.vue';

import { UseRss } from '@/use/Rss.use';

import { FeedArticle } from '@/types/FeedArticle.type';

export default defineComponent({
    name: 'ArticlesFeedComponent',

    components: {
        ArticleComponent,
    },

    setup() {
        const useRss = UseRss();

        const articles = useRss.articles;

        const isArticlesExpanded = ref<boolean>(false);

        const articlesForDisplay = computed<Array<FeedArticle> | null>(() => {
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
            if (articlesForDisplay.value === null)
                return null;

            return articlesForDisplay.value.filter(x => x.publishedAt.isToday());
        });

        const articlesYesterday = computed<Array<FeedArticle> | null>(() => {
            if (articlesForDisplay.value === null)
                return null;

            return articlesForDisplay.value.filter(x => x.publishedAt.isYesterday());
        });

        const articlesPrevious = computed<Array<FeedArticle> | null>(() => {
            if (articlesForDisplay.value === null)
                return null;

            return articlesForDisplay.value
                .filter(x => !x.publishedAt.isToday() && !x.publishedAt.isYesterday());
        });

        return {
            articlesForDisplay,
            articlesToday,
            articlesYesterday,
            articlesPrevious,

            expandArticles() {
                isArticlesExpanded.value = true;
            },
        }
    },
});
</script>

<style lang="scss">
.articles-feed-component {
    h2 {
        margin: 2.5rem 0;
    }

    .expand-container {
        padding: 2rem 0;
    }
}
</style>
