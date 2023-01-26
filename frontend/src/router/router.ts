import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

import FeedView from '@/view/feed/Feed.view.vue';

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        component: FeedView,
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export { router }