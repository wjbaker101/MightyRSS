export type EventKey =
    'ON_LOG_IN' |
    'TRIGGER_LOG_OUT';

type EventFunction<T> = (params: T) => void;

const events: Record<EventKey, Array<EventFunction<any>>> = {
    ON_LOG_IN: [],
    TRIGGER_LOG_OUT: [],
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