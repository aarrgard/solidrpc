using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Model
{
    /// <summary>
    /// Represents a proxies json file
    /// </summary>
    public class AzProxies
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public AzProxies()
        {
            Proxies = new List<KeyValuePair<string, AzProxy>>();
        }

        /// <summary>
        /// The schema
        /// </summary>
        [DataMember(Name = "$schema", EmitDefaultValue = false)]
        public string Schema { get; set; }

        /// <summary>
        /// The schema
        /// </summary>
        [DataMember(Name = "proxies", EmitDefaultValue = false)]
        public IList<KeyValuePair<string, AzProxy>> Proxies { get; set; }

        //public void SetSortedProxies()
        //{
        //    var sortedProxies = Proxies as OrderedDictionary<string, AzProxy>;
        //    if(sortedProxies == null)
        //    {
        //        sortedProxies = new OrderedDictionary<string, AzProxy>(Proxies);
        //        Proxies = sortedProxies;
        //    }
        //}
    }
}
