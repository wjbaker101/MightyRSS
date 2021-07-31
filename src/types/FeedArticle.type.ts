import { Dayjs } from 'dayjs';

export interface FeedArticle {
    url: string;
    title: string;
    summary: string;
    author: string;
    publishedAt: Dayjs;
}
