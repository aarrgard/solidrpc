using SolidRpc.Abstractions.OpenApi.OAuth2;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Class responsible for configuring the authorities
    /// </summary>
    public class AuthorityConfigurator
    {
        private Action<IAuthority> rootConf = a => { };

        /// <summary>
        /// Adds a configuration callback for supplied authority.
        /// </summary>
        /// <param name="conf"></param>
        public void Configure(Action<IAuthority> conf)
        {
            if (conf == null) return;
            var oldConf = rootConf;
            Action<IAuthority> newConf = a => {
                conf(a);
                oldConf(a); 
            };
            rootConf = newConf;
        }

        /// <summary>
        /// Configures suppplied authority
        /// </summary>
        /// <param name="authorityImpl"></param>
        /// <returns></returns>
        public AuthorityImpl Configure(AuthorityImpl authorityImpl)
        {
            rootConf(authorityImpl);
            return authorityImpl;
        }
    }
}
