import { ref, watch } from 'vue';

import { feedService } from '@/service/Feed.service';

import { FeedArticle } from '@/types/FeedArticle.type';
import { FeedSource } from '@/types/FeedSource.type';

const articles = ref<Array<FeedArticle> | null>(null);
const feeds = ref<Array<FeedSource> | null>(null);

watch(articles, (value) => {
    if (value === null) {
        feeds.value = null;
        return;
    }

    const uniqueFeeds = new Map<string, FeedSource>();
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
