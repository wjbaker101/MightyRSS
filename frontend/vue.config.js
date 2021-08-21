module.exports = {
    outputDir: '../backend/wwwroot',

    css: {
        loaderOptions: {
            sass: {
                prependData: `@import 'src/style/global-inject.scss';`,
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
        disableHostCheck: true,
    },

    pwa: {
        name: 'MightyRSS',
        themeColor: '#be9342',
        msTileColor: '#be9342',
        appleMobileWebAppCapable: 'yes',
        manifestOptions: {
            'background_color': '#be9342',
        },
    },
}
