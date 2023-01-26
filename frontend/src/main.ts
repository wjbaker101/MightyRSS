import { createApp } from 'vue';

import '@/setup/DayJS';
import '@/setup/ServiceWorker';
import { components } from '@/setup/components';

import App from '@/App.vue';
import { router } from '@/router/router';

const app = createApp(App);
app.use(router);
app.use(components);
app.mount('#app');