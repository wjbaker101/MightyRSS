import dayjs from 'dayjs';

import { IApiCollection } from '@/api/types/ApiCollection.type';
import { ICollection } from '@/model/Collection.model';

export const collectionMapper = {

    map(collection: IApiCollection): ICollection {
        return {
            reference: collection.reference,
            createdAt: dayjs(collection.createdAt),
            name: collection.name,
        };
    },

};