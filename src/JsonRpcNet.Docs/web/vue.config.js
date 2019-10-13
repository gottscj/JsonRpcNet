module.exports = {
    chainWebpack: config => {
        config.plugin('copy').tap(options => {
            if (process.env.NODE_ENV === "production") {
                options[0][0].ignore.unshift("jsonRpcApi.json");
            }
            return options;
        })
    },
    productionSourceMap: false
}