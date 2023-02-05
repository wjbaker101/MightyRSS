export interface IFeedSource {
    reference: string;
    title: string;
    description: string;
    rssUrl: string;
    websiteUrl: string;
    collection: string | null;
}