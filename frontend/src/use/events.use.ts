export type EventKey =
    'ON_LOG_IN';

type EventFunction = (params: object) => void;

const events = new Map<EventKey, Array<EventFunction>>();
events.set('ON_LOG_IN', []);

export const useEvents = function () {
    return {

        publish(key: EventKey, params: object): void {
            events.get(key)?.forEach(x => x(params))
        },

        subscribe(key: EventKey, func: EventFunction): void {
            events.get(key)?.push(func);
        },

    };
};