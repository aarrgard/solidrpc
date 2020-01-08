﻿import React from "react";
import ReactDOM from "react-dom";
import { UserManager, Log } from 'oidc-client';
import "./styles.css";
import { Security } from 'solidrpc-security';
import { UserInfo } from "./userinfo";
import { Login } from "./components/login";
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

Security.Services.Oidc.OidcClientInstance.Settings().subscribe(settings => {
    console.log('Got settings from oidc client service:');
    console.log(settings);
    var userManager = new UserManager(settings);
    Log.logger = console;
    userManager.signinPopup().then(o => {
        console.log('logged in?:');
        console.log(o);
    });
    //userManager.querySessionStatus().then(o => {
    //    console.log('Session status:');
    //    console.log(o);
    //});

})

const rootElement = document.getElementById("root");
ReactDOM.render(<App />, rootElement);
