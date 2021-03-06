export interface FeedSource {
    reference: string;
    title: string;
    description: string;
    rssUrl: string;
    websiteUrl: string;
    collection: string | null;
    titleAlias: string | null;
}
