import { createApp } from 'vue';

import '@/setup/dayjs-extends';
import '@/setup/service-worker';
import '@/setup/events';

import App from '@/App.vue';
import { router } from '@/router/router';
import { components } from '@/setup/components';

const app = createApp(App);
app.use(router);
app.use(components);
app.mount('#app');