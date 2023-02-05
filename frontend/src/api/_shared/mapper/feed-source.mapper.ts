import { IFeedSource } from '@/model/FeedSource.model';
import { IApiFeedSource } from '../ApiFeedSource.type';

export const feedSourceMapper = {

    map(feedSource: IApiFeedSource): IFeedSource {
        return {
            reference: feedSource.reference,
            title: feedSource.title,
            description: feedSource.description,
            rssUrl: feedSource.rssUrl,
            websiteUrl: feedSource.websiteUrl,
            collection: feedSource.collection,
        };
    },

};