import { createApp } from 'vue';

import '@/setup/DayJS';
import '@/setup/ServiceWorker';

import App from '@/App.vue';

const app = createApp(App);

app.mount('#app');