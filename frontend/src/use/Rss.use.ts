import { readonly, ref } from 'vue';
import dayjs from 'dayjs';

import { apiClient } from '@/api/api-client';

import { IFeedArticle } from '@/model/FeedArticle.model';
import { IFeedSource } from '@/model/FeedSource.model';

const feed = ref<Array<IFeedArticle> | null>(null);
const feedSources = ref<Array<IFeedSource> | null>(null);

export const useRss = function () {
    return {

        feed: readonly(feed),

        sources: readonly(feedSources),

        async refresh(): Promise<void> {
            const result = await apiClient.feed.get();
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
                        reference: source.feedSource.reference,
                        title: source.feedSource.title,
                        description: source.feedSource.description,
                        rssUrl: source.feedSource.rssUrl,
                        websiteUrl: source.feedSource.websiteUrl,
                        collection: source.feedSource.collection,
                    },
                })))
                .flat();

            feedSources.value = result.sources
                .map(x => ({
                    reference: x.feedSource.reference,
                    title: x.feedSource.title,
                    description: x.feedSource.description,
                    rssUrl: x.feedSource.rssUrl,
                    websiteUrl: x.feedSource.websiteUrl,
                    collection: x.feedSource.collection,
                }))
                .sort((a, b) => a.title.localeCompare(b.title));
        },

        async addSource(url: string): Promise<void> {
            const result = await apiClient.feed.addSource({
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
                    reference: result.feedSource.reference,
                    title: result.feedSource.title,
                    description: result.feedSource.description,
                    rssUrl: result.feedSource.rssUrl,
                    websiteUrl: result.feedSource.websiteUrl,
                    collection: result.feedSource.collection,
                },
            }));

            feed.value = feed.value?.concat(articles) ?? null;
        },

        async deleteSource(reference: string): Promise<void> {
            const result = await apiClient.feed.deleteSource(reference);
            if (result instanceof Error)
                return;

            const source = feedSources.value?.findIndex(x => x.reference === reference);
            if (source)
                feedSources.value?.splice(source);
        },

    };
};