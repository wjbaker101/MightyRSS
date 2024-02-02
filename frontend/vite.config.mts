import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import path from 'path';

export default defineConfig({

    plugins: [
        vue(),
    ],

    resolve: {
        alias: [
            {
                find: /^~(.*)$/,
                replacement: '$1',
            },
            {
                find: '@',
                replacement: path.resolve(__dirname, 'src'),
            }
        ],
    },

    optimizeDeps: {
        exclude: [
            '@wjb/vue/use/modal.use',
            '@wjb/vue/use/popup.use',
        ],
    },

    server: {
        proxy: {
            '/api': {
                target: 'https://localhost:44394',
                secure: false,
                changeOrigin: true,
            },
        },
    },

});