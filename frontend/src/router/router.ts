import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

import ConfigurationView from '@/view/configuration/Configuration.view.vue';
import FeedView from '@/view/feed/Feed.view.vue';

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        component: FeedView,
    },
    {
        path: '/configuration',
        component: ConfigurationView,
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export { router }