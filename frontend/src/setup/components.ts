import { Plugin } from 'vue';

import IconComponent from '@wjb/vue/component/IconComponent.vue';

export const components: Plugin = {

    install(app) {
        app.component('IconComponent', IconComponent);
    },

};