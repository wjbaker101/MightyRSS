import dayjs from 'dayjs';

import { feedApi } from '@/api/feed/Feed.api';

import { AddFeedSourceRequest } from '@/api/feed/types/AddFeedSource.type';
import { FeedSource } from '@/types/FeedSource.type';

class FeedService {

    async getFeed(): Promise<Array<FeedSource> | Error> {
        const getFeedResponse = await feedApi.getFeed();

        if (getFeedResponse instanceof Error)
            return getFeedResponse;

        return getFeedResponse.sources.map(source => ({
            reference: source.reference,
            title: source.title,
            description: source.description,
            rssUrl: source.rssUrl,
            websiteUrl: source.websiteUrl,
            articles: source.articles.map(article => ({
                url: article.url,
                title: article.title,
                summary: article.summary,
                author: article.author,
                publishedAt: dayjs(article.publishedAt ?? article.publishedAtAsString),
            })),
        }));
    }

    async addFeedSource(request: AddFeedSourceRequest): Promise<FeedSource | Error> {
        const addFeedSourceResponse = await feedApi.addFeedSource(request);

        if (addFeedSourceResponse instanceof Error)
            return addFeedSourceResponse;

        return {
            reference: addFeedSourceResponse.reference,
            title: addFeedSourceResponse.title,
            description: addFeedSourceResponse.description,
            rssUrl: addFeedSourceResponse.rssUrl,
            websiteUrl: addFeedSourceResponse.websiteUrl,
            articles: addFeedSourceResponse.articles.map(x => ({
                url: x.url,
                title: x.title,
                summary: x.summary,
                author: x.author,
                publishedAt: dayjs(x.publishedAt ?? x.publishedAtAsString),
            })),
        };
    }
}

export const feedService = new FeedService();
