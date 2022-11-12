export const Keys: Record<string, string> = {
    LOGIN_TOKEN: 'LOGIN_TOKEN',
};

interface ICacheItem<T> {
    data: T;
    expiresAt: number;
}

export const useCache = function () {
    return {

        get<T>(key: string): T | null {
            const storage = localStorage.getItem(key);
            if (storage === null)
                return null;

            const parsed = JSON.parse(storage) as ICacheItem<T>;

            if (parsed.expiresAt !== -1 && Date.now() > parsed.expiresAt)
                return null;

            return parsed.data;
        },

        set<T>(key: string, data: T, expiresAt: number = -1): void {
            const cacheItem: ICacheItem<T> = {
                data,
                expiresAt,
            };
            localStorage.setItem(key, JSON.stringify(cacheItem));
        },

        clear(key: string): void {
            localStorage.removeItem(key);
        },

    };
};