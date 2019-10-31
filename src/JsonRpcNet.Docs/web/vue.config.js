module.exports = {
    productionSourceMap: false,
    css: {
        loaderOptions: {
            sass: {
                prependData: `@import "./src/assets/theme.scss";`
            }
        }
    },
    chainWebpack: config => {
        config.plugin('copy').tap(options => {
            if (process.env.NODE_ENV === "production") {
                options[0][0].ignore.unshift("jsonRpcApi.json");
                options[0][0].ignore.unshift("config.json");
            }
            return options;
        })
    }
}