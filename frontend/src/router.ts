import { createRouter, createWebHistory, NavigationGuardWithThis, RouteRecordRaw } from 'vue-router';

import ConfigurationView from '@/views/configuration/Configuration.view.vue';
import LoginView from '@/views/login/Login.view.vue';
import FeedView from '@/views/feed/Feed.view.vue';

import { useAppData } from '@/use/app-data.use';

const appData = useAppData();

const requireAuth: NavigationGuardWithThis<void> = function (to, from, next): void {
    if (appData.auth.loginToken.value === null)
        next('/login');
    else
        next();
};

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        component: FeedView,
        beforeEnter: [requireAuth],
    },
    {
        path: '/login',
        component: LoginView,
    },
    {
        path: '/configuration',
        component: ConfigurationView,
        beforeEnter: [requireAuth],
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export { router }