"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var types_1 = require("./types");
var cancellationtoken_1 = require("cancellationtoken");
var x = new types_1.Security.Services.SolidRpcSecurityImpl(function (s) { return s.replace('https://localhost/', 'http://localhost:5000/'); });
var sub = x.LoginPage(cancellationtoken_1.default.create().token);
//var sub = x.CreateTypesTs('SolidRpc.Security', CancellationToken.create().token);
sub.subscribe(function (item) {
    var ns = item;
    //ns = item.Namespaces.filter(o => o.Name == "SolidRpc")[0];
    //ns = ns.Namespaces.filter(o => o.Name == "IO")[0];
    console.log(ns);
}, function (error) {
    console.log(error);
});
//# sourceMappingURL=test.js.map