"use strict";
var path = require('path');

module.exports = function (callback, npmPath, npmCommand) {
    console.log("Using npm located @ " + npmPath);

    console.log("Invoking npm command " + npmCommand + " in folder " + process.cwd());

    var npmArgs = [];
    for (var i = 0; i < arguments.length; i++) {
        if (i > 2) {
            npmArgs.push(arguments[i]);
        }
    }

    var unsupported = require(path.join(npmPath, 'lib/utils/unsupported.js'));
    unsupported.checkForBrokenNode();

    var log = require(path.join(npmPath, 'node_modules/npmlog'));
    log.pause(); // will be unpaused when config is loaded.
    log.info('it worked if it ends with', 'ok');

    unsupported.checkForUnsupportedNode();

    var npm = require(path.join(npmPath, 'lib/npm.js'));
    var npmconf = require(path.join(npmPath, 'lib/config/core.js'));
    var errorHandler = require(path.join(npmPath, 'lib/utils/error-handler.js'));

    var configDefs = npmconf.defs;
    var shorthands = configDefs.shorthands;
    var types = configDefs.types;
    var nopt = require(path.join(npmPath, 'node_modules/nopt'));

    var conf = nopt(types, shorthands, [process.argv[0], process.argv[1], '--strict-ssl', 'false']);
    console.log(JSON.stringify(conf));
    npm.command = npmCommand;
    npm.argv = npmArgs;
    npm.load(conf, function (ex) {
        if (ex) {
            callback(null, 1);
            return errorHandler(ex);
        }
        console.log('npm command:' + npm.command);
        console.log('npm argv:');
        npm.argv.forEach(function (o) {
            console.log('  - ' + o);
        });
        npm.commands[npm.command](npm.argv, function (err) {
            if (err) {
                console.log("NPM command error:" + err);
                callback(null, 1);
                return;
            }
            console.log("NPM command successful");
            callback(null, 0);
            return;
        });
    });
 };
