const path = require('path');
const CopyPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: './assets/index.js',
    output: {
        path: path.resolve(__dirname, "wwwroot"),
        filename: 'bundle.js'
    },
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: 'styles.css'
                        }
                    },
                    {
                        loader: 'extract-loader'
                    },
                    {
                        loader: 'css-loader?-url'
                    },
                    {
                        loader: 'postcss-loader'
                    },
                    {
                        loader: 'sass-loader'
                    }
                ]
            },
            {
                test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]',
                        outputPath: 'fonts'
                    }
                }]
            }
        ]
    },
    plugins: [
        new CopyPlugin({
            patterns: [
                {from: './node_modules/@fortawesome/fontawesome-free/webfonts', to: 'fonts'}
            ]
        })
    ]
}