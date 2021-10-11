using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnAbp.AspNetCore.MultiProject.MultiProject
{
    public class AspNetCoreMultiProjectOptions
    {
        /// <summary>
        /// Default: <see cref="TenantResolverConsts.DefaultTenantKey"/>.
        /// </summary>
        public string ProjectKey { get; set; }

        public AspNetCoreMultiProjectOptions()
        {
            ProjectKey = "_project";
        }
    }
}
