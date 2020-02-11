import React from "react";
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
//console.log('Setting up login scripts');
//Security.Services.SolidRpcSecurityInstance.LoginScripts().subscribe(scripts => {
//    console.log('Scripts received2');
//    var head = document.getElementsByTagName('head')[0];
//    scripts.forEach(src => {
//        var s = document.createElement("script");
//        s.type = "text/javascript";
//        s.src = src;
//        head.append(s);
//    });
//});

console.log('Setting up oidc client');
Security.Services.Oidc.OidcClientInstance.Settings().subscribe(settings => {
    console.log('Got settings from oidc client service:');
    console.log(settings);

    let json = JSON.parse(settings.toJson());
    console.log(json)
    var userManager = new UserManager(json);
    var url;
    var keepOpen = false;
    userManager.signoutPopupCallback(url, keepOpen).then(_ => {
        console.log('signoutPopupCallback:' + url);
    }).catch(error => {
        console.error(error);
    });
    userManager.signinPopupCallback(url).then(result => {
        console.log('signinPopupCallback:' + url);
        console.log('signinPopupCallback:' + window.location.href);
        console.log(result);
    }).catch(error => {
        console.error(error);
    });
    userManager.signinSilentCallback().then(() => {
        console.log('signinSilentCallback');
    }).catch(error => {
        console.error(error);
    });;

    userManager.events.addUserLoaded(o => {
        console.log('UserLoaded!');
        console.log(o);
    });
    Log.logger = console;
    Log.level = Log.DEBUG;
    userManager.signinPopup().then(o => {
        console.log('logged in?:');
        console.log(o);
    }).catch(error => {
        console.error(error);
    });
    userManager.querySessionStatus().then(o => {
        console.log('Session status:');
        console.log(o);
    }).catch(error => {
        console.error(error);
    });

})

const rootElement = document.getElementById("root");
ReactDOM.render(<App />, rootElement);
