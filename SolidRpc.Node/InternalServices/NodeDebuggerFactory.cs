using SolidRpc.Abstractions;
using SolidRpc.Node.InternalServices;
using System;

[assembly: SolidRpcService(typeof(INodeDebuggerFactory), typeof(NodeDebuggerFactory), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.Node.InternalServices
{
    public class NodeDebuggerFactory : INodeDebuggerFactory
    {
    }
}
