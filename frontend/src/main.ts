import { createApp } from 'vue';

import '@/setup/DayJS';
import '@/setup/ServiceWorker';
import { components } from '@/setup/components';

import App from '@/App.vue';

const app = createApp(App);
app.use(components);
app.mount('#app');