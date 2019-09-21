import React from "react";

export default class Login extends React.Component {
    handleClick = () => {
        var doLoginName = 'solidRpcDo' + this.props.provider + 'Login';
        var doLogin = window[doLoginName];
        if (typeof doLogin === 'function') {
            doLogin();
        }
    };

    render() {
        return (
            <div
                onClick={this.handleClick}
                className={'login.button.' + this.props.provider}
            >
                Login to {this.props.provider}
            </div>
        );
    }
}
