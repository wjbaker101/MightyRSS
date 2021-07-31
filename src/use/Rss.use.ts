import { ref } from 'vue';

import { FeedArticle } from '@/types/FeedArticle.type';
import { FeedSource } from '@/types/FeedSource.type';

const articles = ref<Array<FeedArticle> | null>(null);
const sources = ref<Array<FeedSource> | null>(null);

export function UseRss() {
    return {
        articles,
        sources,
    }
}
