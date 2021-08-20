import { Dayjs } from 'dayjs';

import { FeedSource } from '@/types/FeedSource.type';

export interface FeedArticle {
    url: string;
    source: FeedSource;
    title: string;
    summary: string;
    author: string;
    publishedAt: Dayjs;
}
