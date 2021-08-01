import { ref } from 'vue';

import { FeedArticle } from '@/types/FeedArticle.type';

const articles = ref<Array<FeedArticle> | null>(null);

export function UseRss() {
    return {
        articles,
    }
}
