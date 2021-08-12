module.exports = {
    outputDir: '../mighty-rss-api/wwwroot',

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
}
