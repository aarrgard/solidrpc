import React from "react";
import ReactDOM from "react-dom";
import { UserManager } from 'oidc-client';
import "./styles.css";
import { Security } from 'solidrpc-security';
import { UserInfo } from "./userinfo";
import { Login } from "./login";
import { SolidRpc } from 'solidrpc';

function App() {
    return (
        <div className="app">
            <UserInfo />
            <Login />
        </div>
    );
}

//
// load scripts
//
Security.Services.SolidRpcSecurityInstance.LoginScripts().subscribe(scripts => {
    console.log('Scripts received2');
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
