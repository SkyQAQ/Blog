const path = require('path')

function resolve(dir) {
    return path.join(__dirname, dir)
}
module.exports = {
    baseUrl: '',
    outputDir: 'build',
    assetsDir: './static',

    // 设置为 true 时，eslint-loader 会将 lint 错误输出为编译警告。默认情况下，警告仅仅会被输出到命令行，且不会使得编译失败。
    // 设置为生产环境不启用
    lintOnSave: process.env.NODE_ENV !== 'production',

    // 配置WebPack
    configureWebpack: config => {
        config.resolve = {
            extensions: ['.js', '.vue', '.json', ".css"],
            alias: {
                'vue$': 'vue/dist/vue.js',
                '@': resolve('src'),
            }
        }
    },
    devServer: {
        host: 'localhost',
        port: '8888',
        //开发时，前端与后端不在一个服务器
        proxy: {
            '/api': {
                target: 'http://localhost:1003',
                ws: true,
                changeOrigin: true // 跨域
            }
        },

        https: false,
        overlay: {
            warnings: true,
            errors: true
        },
    },
    runtimeCompiler: undefined,
    productionSourceMap: false,
    parallel: undefined,
    css: undefined
}