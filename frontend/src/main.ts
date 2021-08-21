import { createApp } from 'vue';

import '@/setup/DayJS';
import '@/setup/ServiceWorker';

import App from '@/App.vue';

createApp(App)
    .mount('#app');
