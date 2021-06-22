import { CodeGenerator } from 'solidrpc.tests';
(async function () {
	return await CodeGenerator.TestInterfaceInstance.ProxyIntegerAsync(4711).toPromise();
})()
