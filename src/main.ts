import { createApp } from 'vue';

import App from '@/App.vue';

import dayjs from 'dayjs';
import advancedFormat from 'dayjs/plugin/advancedFormat';
import relativeTime  from 'dayjs/plugin/relativeTime';
import isToday  from 'dayjs/plugin/isToday';
import isYesterday  from 'dayjs/plugin/isYesterday';

createApp(App)
    .mount('#app');

dayjs.extend(advancedFormat);
dayjs.extend(relativeTime);
dayjs.extend(isToday);
dayjs.extend(isYesterday);
