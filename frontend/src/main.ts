import { createApp } from 'vue';

import '@/setup/DayJS';
import '@/setup/ServiceWorker';
import { setupComponents } from '@/setup/components';

import App from '@/App.vue';

const app = createApp(App);

setupComponents(app);

app.mount('#app');