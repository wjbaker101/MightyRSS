import { createApp } from 'vue';

import '@/setup/dayjs.setup';
import '@/setup/service-worker';
import '@/setup/events';

import App from '@/App.vue';
import { router } from '@/router';
import { components } from '@/setup/components.setup';

const app = createApp(App);
app.use(router);
app.use(components);
app.mount('#app');