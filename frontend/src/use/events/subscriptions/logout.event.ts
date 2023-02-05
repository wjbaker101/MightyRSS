import { useAppData } from '@/use/app-data.use';
import { useEvents } from '@/use/events/events.use';

const appData = useAppData();
const events = useEvents();

events.subscribe('TRIGGER_LOG_OUT', () => {
    appData.auth.logOut();
});