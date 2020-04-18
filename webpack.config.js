const path = require("path")
var webpack = require("webpack");

module.exports = (env, argv) => {
    const mode = argv.mode;
    return {
        mode: mode,
        entry: "./src/App.fsproj",
        devServer: {
            contentBase: path.join(__dirname, "./dist"),
            hot: true,
            inline: true
        },
        plugins: mode === "development" ?
            // development mode plugins
            [
                new webpack.HotModuleReplacementPlugin()
            ]
            :
            // production mode plugins
            [],
        module: {
            rules: [{
                test: /\.fs(x|proj)?$/,
                use: {
                    loader: "fable-loader",
                    options: {
                        define: mode === "development" ? ["DEVELOPMENT"] : []
                    }
                }
            }]
        }
    }
}