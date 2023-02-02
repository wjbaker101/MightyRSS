import { useAppData } from '@/use/app-data.use';
import { useEvents } from '@/use/events.use';

const appData = useAppData();
const events = useEvents();

events.subscribe('ON_LOG_OUT', () => {
    appData.auth.logOut();
});