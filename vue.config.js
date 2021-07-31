module.exports = {
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
