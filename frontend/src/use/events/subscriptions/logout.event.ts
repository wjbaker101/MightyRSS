import { useAuth } from '@/use/auth.use';
import { useEvents } from '@/use/events/events.use';

const auth = useAuth();
const events = useEvents();

events.subscribe('TRIGGER_LOG_OUT', () => {
    auth.logOut();
    window.location.href = '/login';
});