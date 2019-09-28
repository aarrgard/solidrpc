import React from "react";
import { SolidRpc } from 'solidrpc';
import { Security } from 'solidrpc-security';

export default class Login extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            provider: props.provider
        };
     }

    handleClick = () => {
        let action = 'Login';
        if (this.state.provider.Status === 'LoggedIn') {
            action = 'Logout';
        }
        var doLoginName = 'solidRpcDo' + this.props.provider.Name + action;
        var doLogin = window[doLoginName];
        if (typeof doLogin === 'function') {
            doLogin();
        } else {
            console.error('Cannot find function:' + doLoginName);
        }
        Security.Services.SolidRpcSecurityInstance.LoginProviders().subscribe();
    };

    render() {
        let action = 'Login to';
        if (this.state.provider.Status === 'LoggedIn') {
            action = 'Logout from';
        }
        return (
            <div
                onClick={this.handleClick}
                className={'login.button.' + this.state.provider.Name}
            >
                {action} {this.state.provider.Name}
            </div>
        );
    }
}
