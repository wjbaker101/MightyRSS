import { FeedArticle } from '@/types/FeedArticle.type';

export interface FeedSource {
    reference: string;
    description: string;
    rssUrl: string;
    websiteUrl: string;
    articles: Array<FeedArticle>;
}
