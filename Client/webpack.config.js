var path = require('path');
var HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
    entry: './src/index.tsx',
    output: {
        path: path.resolve('dist'),
        filename: 'bundle.js'
    },
    resolve: {
        extensions: ['.js', '.jsx', '.less', '.tsx', '.ts']
    },
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'babel-loader',
                options: {  
                    presets: ['react', 'es2015', 'stage-3'],
                    plugins: [
                        ['import', {
                            "libraryName": "antd",
                            "libraryDirectory": "lib"
                        }],
                    ]
                }
            },
            {
                test: /\.css$/,
                use: ['css-loader']
            },
            {
                test: /\.(png|jpg|jpeg|gif|svg)$/,
                loader: 'url-loader?limit=25000'
            },
            {
                test: /\.tsx?$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'babel-loader',
                options: {  
                    presets: ['react', 'es2015', 'stage-3'],
                    plugins: [
                        ['import', {
                            "libraryName": "antd",
                            "libraryDirectory": "lib"
                        }],
                    ]
                }
            },
            {
                test: /\.ts?$/,
                include: /client/,
                use: 'awesome-typescript-loader?silent=true'
            },
            {
                test: /\.less$/,
                use: [{
                        loader: "style-loader"
                    },
                    {
                        loader: "css-loader"
                    },
                    {
                        loader: "less-loader",
                        options: {
                            javascriptEnabled: true
                        }
                    }
                ]
            }
        ]
    },
    plugins: [new HtmlWebpackPlugin({
        template: './src/index.html',
        filename: 'index.html',
        inject: 'body'
    })],
    devServer: {
        historyApiFallback: true,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "GET, POST, PUT, DELETE, PATCH, OPTIONS",
            "Access-Control-Allow-Headers": "X-Requested-With, content-type, Authorization",
            "Access-Control-Allow-Credentials":"true"
            }
    }
}