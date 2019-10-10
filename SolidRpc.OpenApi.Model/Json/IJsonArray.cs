using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json
{
    /// <summary>
    /// Represents a Json array
    /// </summary>
    public interface IJsonArray : IJsonStruct, IList<IJsonStruct>
    {

    }
}
