import React from "react";
import ReactDOM from "react-dom";
import { Security } from 'solidrpc-security';
import { Login } from "./components/login";
import "./styles.css";

function App() {
    return (
        <div className="app">
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

const rootElement = document.getElementById("root");
ReactDOM.render(<App />, rootElement);
