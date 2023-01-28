import { Dayjs } from 'dayjs';

export interface ICollection {
    readonly reference: string;
    readonly createdAt: Dayjs;
    readonly name: string;
}