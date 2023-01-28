import { Dayjs } from 'dayjs';

import { IFeedSource } from '@/model/FeedSource.model';

export interface IFeedArticle {
    url: string;
    source: IFeedSource;
    title: string;
    summary: string;
    author: string;
    publishedAt: Dayjs;
}