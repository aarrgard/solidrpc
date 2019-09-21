import { Security } from './types'
import CancellationToken from 'cancellationtoken';

var x = new Security.Services.SolidRpcSecurityImpl(s=>s.replace('https://localhost/', 'http://localhost:5000/'));
var sub = x.LoginPage(CancellationToken.create().token);
//var sub = x.CreateTypesTs('SolidRpc.Security', CancellationToken.create().token);
sub.subscribe(item => {
    let ns = item;
    //ns = item.Namespaces.filter(o => o.Name == "SolidRpc")[0];
    //ns = ns.Namespaces.filter(o => o.Name == "IO")[0];
    console.log(ns);
}, error => {
    console.log(error);
    });
