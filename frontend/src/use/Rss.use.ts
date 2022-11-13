import { readonly, ref } from 'vue';
import dayjs from 'dayjs';

import { feedApi } from '@/api/feed/Feed.api';

import { IFeedArticle } from '@/model/FeedArticle.type';
import { IFeedSource } from '@/model/FeedSource.type';

const feed = ref<Array<IFeedArticle> | null>(null);
const feedSources = ref<Array<IFeedSource> | null>(null);

export const useRss = function () {
    return {

        feed: readonly(feed),

        sources: readonly(feedSources),

        async refresh(): Promise<void> {
            const result = await feedApi.getFeed();
            if (result instanceof Error)
                return;

            feed.value = result.sources.map(source => source.articles
                .map(article => ({
                    url: article.url,
                    title: article.title,
                    summary: article.summary,
                    author: article.author,
                    publishedAt: dayjs(article.publishedAt ?? article.publishedAtAsString),
                    source: {
                        reference: source.reference,
                        title: source.title,
                        description: source.description,
                        rssUrl: source.rssUrl,
                        websiteUrl: source.websiteUrl,
                        collection: source.collection,
                        titleAlias: source.titleAlias,
                    },
                })))
                .flat();

            feedSources.value = result.sources
                .map(x => ({
                    reference: x.reference,
                    title: x.title,
                    description: x.description,
                    rssUrl: x.rssUrl,
                    websiteUrl: x.websiteUrl,
                    collection: x.collection,
                    titleAlias: x.titleAlias,
                }))
                .sort((a, b) => (a.titleAlias ?? a.title).localeCompare(b.titleAlias ?? b.title));
        },

        async addSource(url: string): Promise<void> {
            const result = await feedApi.addFeedSource({
                url,
            });
            if (result instanceof Error)
                return;

            const articles: Array<IFeedArticle> = result.articles.map(x => ({
                url: x.url,
                title: x.title,
                summary: x.summary,
                author: x.author,
                publishedAt: dayjs(x.publishedAt ?? x.publishedAtAsString),
                source: {
                    reference: result.reference,
                    title: result.title,
                    description: result.description,
                    rssUrl: result.rssUrl,
                    websiteUrl: result.websiteUrl,
                    collection: result.collection,
                    titleAlias: null,
                },
            }));

            feed.value = feed.value?.concat(articles) ?? null;
        },

        async deleteSource(reference: string): Promise<void> {
            const result = await feedApi.deleteFeedSource(reference);
            if (result instanceof Error)
                return;

            const source = feedSources.value?.findIndex(x => x.reference === reference);
            if (source)
                feedSources.value?.splice(source);
        },

    };
};