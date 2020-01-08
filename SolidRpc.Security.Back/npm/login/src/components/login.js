import React from "react";
import { Security } from 'solidrpc-security';
import { LoginProvider } from './login-provider'

export class Login extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            providers: []
        };
    }

    componentDidMount() {
        Security.Services.SolidRpcSecurityInstance.LoginProviders().subscribe(o => {
            this.setState({ providers: o });
        });
    }

    createProviders() {
        console.log('createProviders');
        let buttons = [];
        for (let idx in this.state.providers) {
            var provider = this.state.providers[idx];
            buttons.push(<LoginProvider key={provider.Name} provider={provider} />);
        }
        return buttons;
    }

    render() {
        return (
            <div className='center'>
                <div className='providers' >
                    {this.createProviders()}
                </div>    
            </div>
        );
    }
}
