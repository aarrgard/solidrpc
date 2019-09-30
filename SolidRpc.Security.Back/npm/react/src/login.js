import React from "react";
import LoginButton from "./components/login-button";

import { Security  } from "solidrpc-security";

export class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            providers: []
        };
        Security.Services.SolidRpcSecurityInstance.LoginProvidersObservable.subscribe(providers => {
            this.setState({ providers: providers });
        });
        Security.Services.SolidRpcSecurityInstance.LoginProviders().subscribe();
    }

    createButtons() {
        let buttons = [];
        for (let idx in this.state.providers) {
            var provider = this.state.providers[idx];
            buttons.push(<LoginButton key={provider.Name} provider={provider} />);
        }
        return buttons;
    }

    render() {
        console.log('rendering login buttons');
        return (
            <div className="login">
                {this.createButtons()}
            </div>
        );
    }
}

