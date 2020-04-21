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
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.(svg|otf)$/,
                loader: 'file-loader?limit=10000&name=assets/images/[name].[ext]',
            },
            {
                test: /\.(png|gif)$/,
                use: [
                    {
                        loader: 'url-loader',
                        options: {
                            limit: 10000,
                            name: 'assets/images/[name].[ext]'
                        }
                    }
                ]
            },
            {
                test: /\.(eot|ttf|woff|woff2)$/,
                loader: 'file-loader?limit=10000&name=assets/fonts/[name].[ext]',
            },]
        }
    }
}