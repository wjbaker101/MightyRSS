const { defineConfig } = require('@vue/cli-service');

module.exports = defineConfig({
    outputDir: '../backend/wwwroot',

    css: {
        loaderOptions: {
            sass: {
                sassOptions: {
                    additionalData: `@import 'src/style/global-inject.scss';`,
                }
            },
        },
    },

    devServer: {
        proxy: {
            '/api': {
                target: 'https://localhost:44394',
                ws: true,
                changeOrigin: true,
            }
        },
    },

    pwa: {
        name: 'MightyRSS',
        themeColor: '#be9342',
        msTileColor: '#be9342',
        appleMobileWebAppCapable: 'yes',
        manifestOptions: {
            'background_color': '#be9342',
            orientation: 'portrait',
        },
    },
});