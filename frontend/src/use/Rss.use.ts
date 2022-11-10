import { ref, watch } from 'vue';

import { feedService } from '@/service/Feed.service';

import { IFeedArticle } from '@/model/FeedArticle.type';
import { IFeedSource } from '@/model/FeedSource.type';

const articles = ref<Array<IFeedArticle> | null>(null);
const feeds = ref<Array<IFeedSource> | null>(null);

watch(articles, (value) => {
    if (value === null) {
        feeds.value = null;
        return;
    }

    const uniqueFeeds = new Map<string, IFeedSource>();
    for (const article of value) {
        const feed = article.source;

        if (!uniqueFeeds.has(feed.reference))
            uniqueFeeds.set(feed.reference, feed);
    }

    feeds.value = [ ...uniqueFeeds.values() ];
});

export function UseRss() {
    return {
        articles,
        feeds,

        async load(): Promise<void | Error> {
            articles.value = null;

            const getFeedResponse = await feedService.getFeed();
            if (getFeedResponse instanceof Error)
                return getFeedResponse;

            articles.value = getFeedResponse;
        },
    }
}
