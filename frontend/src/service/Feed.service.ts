import dayjs from 'dayjs';

import { feedApi } from '@/api/feed/Feed.api';

import { AddFeedSourceRequest } from '@/api/feed/types/AddFeedSource.type';
import { IFeedArticle } from '@/model/FeedArticle.type';

class FeedService {

    async getFeed(): Promise<Array<IFeedArticle> | Error> {
        const getFeedResponse = await feedApi.getFeed();

        if (getFeedResponse instanceof Error)
            return getFeedResponse;

        return getFeedResponse.sources.map(source => source.articles
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
    }

    async addFeedSource(request: AddFeedSourceRequest): Promise<Array<IFeedArticle> | Error> {
        const addFeedSourceResponse = await feedApi.addFeedSource(request);

        if (addFeedSourceResponse instanceof Error)
            return addFeedSourceResponse;

        return addFeedSourceResponse.articles.map(x => ({
            url: x.url,
            title: x.title,
            summary: x.summary,
            author: x.author,
            publishedAt: dayjs(x.publishedAt ?? x.publishedAtAsString),
            source: {
                reference: addFeedSourceResponse.reference,
                title: addFeedSourceResponse.title,
                description: addFeedSourceResponse.description,
                rssUrl: addFeedSourceResponse.rssUrl,
                websiteUrl: addFeedSourceResponse.websiteUrl,
                collection: addFeedSourceResponse.collection,
                titleAlias: null,
            },
        }));
    }

    async deleteFeedSource(reference: string): Promise<void | Error> {
        const deleteFeedSourceResponse = await feedApi.deleteFeedSource(reference);

        if (deleteFeedSourceResponse instanceof Error)
            return deleteFeedSourceResponse;
    }
}

export const feedService = new FeedService();