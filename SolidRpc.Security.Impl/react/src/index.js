import React from "react";
import ReactDOM from "react-dom";
import "./styles.css";

import {
    FacebookLoginButton,
    GoogleLoginButton,
    MicrosoftLoginButton,
} from "react-social-login-buttons";

function App() {
    function handleClick() {
        alert("Hello!");
    }

    return (
        <div className="app">
            <div>
                <FacebookLoginButton onClick={handleClick} />
                <GoogleLoginButton onClick={handleClick} />
                <MicrosoftLoginButton onClick={handleClick} />
            </div>
        </div>
    );
}

const rootElement = document.getElementById("root");
ReactDOM.render(<App />, rootElement);
