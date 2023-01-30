import { Plugin } from 'vue';

import ButtonComponent from '@wjb/vue/component/ButtonComponent.vue';
import IconComponent from '@wjb/vue/component/IconComponent.vue';
import ModalComponent from '@wjb/vue/component/ModalComponent.vue';

export const components: Plugin = {

    install(app) {
        app.component('ButtonComponent', ButtonComponent);
        app.component('IconComponent', IconComponent);
        app.component('ModalComponent', ModalComponent);
    },

};