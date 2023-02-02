import { IApiCollection } from './ApiCollection.type';

export interface ICreateCollectionRequest {
    readonly name: string;
}

export interface ICreateCollectionResponse {
    readonly collection: IApiCollection;
}