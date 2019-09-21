import React from "react";
import ReactDOM from "react-dom";
import { UserManager } from 'oidc-client';
import "./styles.css";
import { Security } from 'solidrpc-security';
import { UserInfo } from "./userinfo";
import { Login } from "./login";

function App() {
    return (
        <div className="app">
            <UserInfo />
            <Login />
        </div>
    );
}


//
// declare namespace
//
let declareNamespace = function (ns) {
    let scope = window;
    ns.split('.').forEach(o => {
        if (typeof scope[o] === 'undefined') {
            console.log('Creating scope ' + o);
            scope[o] = {};
            scope[o].name = o;
            scope[o].parent = scope;
            scope[o].modifiedCallbacks = [];
            scope[o].declareNamespace = declareNamespace;
            scope[o].addModifiedCallback = function (callback) {
                this.modifiedCallbacks.push(callback);
                return this;
            };
            scope[o].modified = function () {
                this.modifiedCallbacks.forEach(o => o());
                return this;
            };
        }
        scope = scope[o];
    });
    return scope;
};

declareNamespace('SolidRpc')
    .addModifiedCallback(() => console.log('scope modified'))
    .modified();

//
// load scripts
//
var sec = new Security.Services.SolidRpcSecurityImpl();
sec.LoginScripts().subscribe(scripts => {
    var head = document.getElementsByTagName('head')[0];
    scripts.forEach(src => {
        var s = document.createElement("script");
        s.type = "text/javascript";
        s.src = src;
        head.append(s);
    });
});

var userManager = new UserManager({
});

const rootElement = document.getElementById("root");
ReactDOM.render(<App />, rootElement);
