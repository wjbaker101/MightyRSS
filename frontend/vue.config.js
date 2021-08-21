module.exports = {
    outputDir: '../backend/wwwroot',

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
        manifestOptions: {
            'background_color': '#be9342',
        },
    },
}
