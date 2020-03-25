using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// Interface that can be implemted in order to receive callbacks when a method binding has been created.
    /// </summary>
    public interface IMethodBindingHandler
    {
        /// <summary>
        /// Invoked the the binding store has created a binding.
        /// </summary>
        /// <param name="binding"></param>
        void BindingCreated(IMethodBinding binding);
    }
}
