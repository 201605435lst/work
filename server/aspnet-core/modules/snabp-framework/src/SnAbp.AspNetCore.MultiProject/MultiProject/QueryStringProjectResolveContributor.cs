using JetBrains.Annotations;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnAbp.AspNetCore.MultiProject.MultiProject
{
    public class QueryStringProjectResolveContributor : HttpProjectResolveContributorBase
    {
        public const string ContributorName = "QueryString";

        public override string Name => ContributorName;
        protected override string GetProjectIdFromHttpContextOrNull([NotNull] IProjectResolveContext context, [NotNull] HttpContext httpContext)
        {
            if (httpContext.Request == null || !httpContext.Request.QueryString.HasValue)
                return null;
            return httpContext.Request.Query["project"];
        }

        private AspNetCoreMultiProjectOptions GetOptions(IProjectResolveContext context)
        {
            return context.ServiceProvider.GetRequiredService<IOptionsSnapshot<AspNetCoreMultiProjectOptions>>().Value;
        }
    }
}
