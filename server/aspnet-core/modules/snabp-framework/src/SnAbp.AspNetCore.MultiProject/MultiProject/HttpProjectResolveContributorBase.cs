using JetBrains.Annotations;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Volo.Abp;

namespace SnAbp.AspNetCore.MultiProject.MultiProject
{
    public abstract class HttpProjectResolveContributorBase : ProjectResolveContributorBase
    {
        public override string Name => throw new NotImplementedException();

        public override void Resolve(IProjectResolveContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext == null) return;
            try
            {
                ResolveFromHttpContext(context, httpContext);
            }
            catch (Exception ex)
            {
                context.ServiceProvider
                    .GetRequiredService<ILogger<HttpProjectResolveContributorBase>>()
                    .LogWarning(ex.ToString());

            }
        }
        protected virtual void ResolveFromHttpContext(IProjectResolveContext context,HttpContext httpContext)
        {
            var projectId = GetProjectIdFromHttpContextOrNull(context, httpContext);
            if (!projectId.IsNullOrEmpty()) context.ProjectId = projectId;
        }

        protected abstract string GetProjectIdFromHttpContextOrNull([NotNull] IProjectResolveContext context, [NotNull] HttpContext httpContext);
    }
}
