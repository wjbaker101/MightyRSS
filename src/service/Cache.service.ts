import { ImmortalStorage, LocalStorageStore, SessionStorageStore } from 'immortal-db';

const cache = new ImmortalStorage([
    LocalStorageStore,
    SessionStorageStore,
]);

interface CacheItem<T> {
    data: T;
    expiresAt: number | null;
}

class CacheService {

    async get<T>(key: string): Promise<T | null> {
        const value = await cache.get(key);
        if (value === null)
            return null;

        const cacheItem: CacheItem<T> = JSON.parse(value);
        if (cacheItem.expiresAt !== null && cacheItem.expiresAt < Date.now())
            return null;

        return cacheItem.data;
    }

    async set<T>(key: string, data: T, expiresIn?: number): Promise<void> {
        const cacheItem: CacheItem<T> = {
            data,
            expiresAt: expiresIn ? Date.now() + expiresIn : null,
        };

        await cache.set(key, JSON.stringify(cacheItem));
    }
}

export const cacheService = new CacheService();

export const CacheKey = {
    LOGIN_TOKEN: 'LOGIN_TOKEN',
};
