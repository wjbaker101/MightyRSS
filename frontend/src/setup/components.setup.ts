import { Plugin } from 'vue';

import { components as wjbComponents } from '@wjb/vue/setup/components';

import CardComponent from '@/components/Card.component.vue';

export const components: Plugin = {

    install(app) {
        app.use(wjbComponents);

        app.component('CardComponent', CardComponent);
    },

};