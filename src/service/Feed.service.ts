import dayjs from 'dayjs';

import { feedApi } from '@/api/feed/Feed.api';

import { AddFeedSourceRequest } from '@/api/feed/types/AddFeedSource.type';
import { FeedArticle } from '@/types/FeedArticle.type';

class FeedService {

    async getFeed(): Promise<Array<FeedArticle> | Error> {
        const getFeedResponse = await feedApi.getFeed();

        if (getFeedResponse instanceof Error)
            return getFeedResponse;

        return getFeedResponse.sources.map(source => source.articles
            .map(article => (<FeedArticle>{
                url: article.url,
                source: source,
                title: article.title,
                summary: article.summary,
                author: article.author,
                publishedAt: dayjs(article.publishedAt ?? article.publishedAtAsString),
            }))
        )
        .flat();
    }

    async addFeedSource(request: AddFeedSourceRequest): Promise<Array<FeedArticle> | Error> {
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
