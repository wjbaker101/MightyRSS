import { readonly, ref } from 'vue';
import dayjs from 'dayjs';

import { feedApi } from '@/api/feed/Feed.api';

import { IFeedArticle } from '@/model/FeedArticle.model';
import { IFeedSource } from '@/model/FeedSource.model';

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
                        reference: source.feedSource.reference,
                        title: source.feedSource.title,
                        description: source.feedSource.description,
                        rssUrl: source.feedSource.rssUrl,
                        websiteUrl: source.feedSource.websiteUrl,
                        collection: source.feedSource.collection,
                        titleAlias: source.feedSource.titleAlias,
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
                    titleAlias: x.feedSource.titleAlias,
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
                    reference: result.feedSource.reference,
                    title: result.feedSource.title,
                    description: result.feedSource.description,
                    rssUrl: result.feedSource.rssUrl,
                    websiteUrl: result.feedSource.websiteUrl,
                    collection: result.feedSource.collection,
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