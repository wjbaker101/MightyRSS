export interface IApiFeedSource {
    readonly reference: string;
    readonly title: string;
    readonly description: string;
    readonly rssUrl: string;
    readonly websiteUrl: string;
    readonly collection: string | null;
    readonly titleAlias: string | null;
}