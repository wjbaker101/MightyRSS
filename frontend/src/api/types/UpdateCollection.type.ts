import { IApiCollection } from '../_shared/ApiCollection.type';

export interface IUpdateCollectionRequest {
    readonly name: string;
}

export interface IUpdateCollectionResponse {
    readonly collection: IApiCollection;
}