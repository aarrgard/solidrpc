import React from "react";
import { Security } from 'solidrpc-security';

export function UserInfo() {
    var secImpl = new Security.Services.SolidRpcSecurityImpl();
    secImpl.LoginProviders().subscribe(res => {
        res.forEach(o => console.log(o.Name));
    });
    return (
        <div className="userinfo">
            <div>
                UserInfo
            </div>
        </div>
    );
}
