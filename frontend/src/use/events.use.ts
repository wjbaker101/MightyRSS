export type EventKey =
    'ON_LOG_IN';

type EventFunction<T> = (params: T) => void;

const events: Record<EventKey, Array<EventFunction<any>>> = {
    ON_LOG_IN: [],
};

export const useEvents = function () {
    return {

        publish<T>(key: EventKey, params: T): void {
            events[key].forEach(x => x(params))
        },

        subscribe<T>(key: EventKey, func: EventFunction<T>): void {
            events[key].push(func);
        },

    };
};