const path = require('path');

module.exports = {
    entry: {
        'SolidRpcSecurity.LoginScript': "./src/SolidRpcSecurity.LoginScript.js",
        'FacebookLocal.LoginScript': "./src/FacebookLocal.LoginScript.js",
        'GoogleLocal.LoginScript': "./src/GoogleLocal.LoginScript.js",
        'MicrosoftLocal.LoginScript': "./src/MicrosoftLocal.LoginScript.js"
    },
	resolve: {
        extensions: [".js"]
    }
};