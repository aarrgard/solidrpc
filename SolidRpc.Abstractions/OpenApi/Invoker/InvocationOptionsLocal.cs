using System;
using System.Threading;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// Connects and disconnects some invocation options 
    /// to/from the local task pipeline
    /// </summary>
    public class InvocationOptionsLocal : IDisposable
    {
        private static AsyncLocal<InvocationOptionsLocal> s_options = new AsyncLocal<InvocationOptionsLocal>();
        
        /// <summary>
        /// 
        /// </summary>
        public static InvocationOptions Current 
        {
            get
            {
                var opts = s_options.Value?.InvocationOptions ?? InvocationOptions.Default;
                return opts;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opts"></param>
        public InvocationOptionsLocal(InvocationOptions invocationOptions)
        {
            InvocationOptions = invocationOptions;
            Parent = s_options.Value;
            s_options.Value = this;
        }

        private InvocationOptionsLocal Parent { get; }

        /// <summary>
        /// 
        /// </summary>
        public InvocationOptions InvocationOptions { get; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            s_options.Value = Parent;
        }
    }
}